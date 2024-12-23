using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public bool showCard = true;
    public List<CardData> allCards = new List<CardData>(); // 玩家完整的卡组（用于永久存储所有卡牌）
    public List<CardData> drawPile = new List<CardData>(); // 抽牌堆
    public List<CardData> discardPile = new List<CardData>(); // 弃牌堆
    public List<CardData> hand = new List<CardData>(); // 手牌
    public int maxHandSize = 1; // 初始手牌数量

    public GameObject cardUIPrefab; // 卡牌 UI 预制体
    public Transform handPanel; // 用于显示手牌的 UI 面板
    public float maxCardSpacing = 100f; // 卡牌之间的最大间隔
    public float maxHandWidth = 600f; // 手牌的最大占用宽度
    public float leftSpace=-250f;//手牌最左侧的位置

    public GameObject discard;
    public static Deck instance;

    public Button discardButton;
    public Button bagButton;
    public Button closeZoneButton;
    public bool InFresh = false; // 在洗牌
    public GameObject CardZone;
    public GameObject CardContent;

    void Awake(){        
        instance = this;
    }

    private void OnDiscardButtonClick(){
        ShowZone(discardPile);
    }

    private void OnBagButtonClick(){
        ShowZone(drawPile);
    }

    private void OnZoneCloseClick(){
        CardZone.SetActive(false);
        for (int i = 0; i < CardContent.transform.childCount; i++) {  
            Destroy (CardContent.transform.GetChild (i).gameObject);  
        }
    }

    private void ShowZone(List<CardData> datas){
        CardZone.SetActive(true);
        for(int i = 0;i != datas.Count; ++i){
            GameObject cardUI = Instantiate(cardUIPrefab, CardContent.transform);
            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            cardUIScript.cardData = datas[i];
            cardUIScript.UpdateCardVisual();
            cardUIScript.CanInteractive = false;
        }
    }


    void Start()
    {
        discardButton.onClick.AddListener(OnDiscardButtonClick);
        bagButton.onClick.AddListener(OnBagButtonClick);
        closeZoneButton.onClick.AddListener(OnZoneCloseClick);
        CardZone.SetActive(false);
        StartBattle();
    }

    public void StartBattle()
    {
        // 初始化战斗中的抽牌堆，将所有卡牌复制到抽牌堆
        drawPile = new List<CardData>(allCards);
        for(int i = 0;i != drawPile.Count; ++i){
            drawPile[i] = Instantiate(allCards[i]);
        }
        Shuffle(drawPile);
        discardPile.Clear();
        hand.Clear();

        // 抽取初始手牌
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard(i);
        }
    }

    public bool DrawCard(int index)
    {
        if(InFresh) return false;

        if (hand.Count >= maxHandSize)
        {
            return true; // 已达到手牌上限
        }

        if (drawPile.Count > 0)
        {
            CardData drawnCard = drawPile[0];
            drawnCard.handIndex = index;
            drawPile.RemoveAt(0);
            hand.Add(drawnCard);

            // 更新手牌 UI
            RectTransform bagRect = bagButton.GetComponent<RectTransform>();
            if(bagRect == null) Debug.Log("bagRect Is NuLL");
            UpdateHandCard(bagRect.anchoredPosition, drawnCard);
            return true;
        }
        return false;
    }
    private void UpdateHandCard(Vector2 Src, CardData data)
    {
        if(!showCard) return;

        // 计算卡牌间距
        float cardSpacing = Mathf.Min(maxCardSpacing, maxHandWidth / Mathf.Max(1, hand.Count));

        GameObject cardUI = Instantiate(cardUIPrefab, handPanel);
        RectTransform cardRect = cardUI.GetComponent<RectTransform>();
        cardRect.anchoredPosition = Src;

        CardUI cardUIScript = cardUI.GetComponent<CardUI>();
        Vector2 Target = new Vector2(leftSpace + data.handIndex* cardSpacing, -375); // 设置卡牌的位置，按顺序排列
        if (cardUIScript != null)
        {
            cardUIScript.cardData = data;
            cardUIScript.UpdateCardVisual();
            cardUIScript.MoveTo(Target, false, false, false);
        }
        else
        {
            Debug.LogWarning("CardUI script is missing on the card UI prefab.");
        }
    }


    public void PlayCard(CardData card)
    {
        if(InFresh) return;
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);

            // 检查是否存在任何无效引用
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            if(!DrawCard(card.handIndex)){
                // 洗牌
                ReshuffleDiscardToDraw(card.handIndex);
            }
        }
    }

    private void ReshuffleDiscardToDraw(int endIndex)//弃牌堆洗回抽牌堆
    {
        if(InFresh) return;
        InFresh = true;
        if (discardPile.Count > 0)
        {
            List<CardData> temp = new List<CardData>(discardPile);
            Shuffle(temp);
            while(temp.Count != 0){
                CardData data = temp[temp.Count-1];
                temp.RemoveAt(temp.Count-1);
                discardPile.Remove(data);

                GameObject cardUI = Instantiate(cardUIPrefab, handPanel);
                RectTransform cardRect = cardUI.GetComponent<RectTransform>();
                cardRect.anchoredPosition = discardButton.GetComponent<RectTransform>().anchoredPosition;

                CardUI cardUIScript = cardUI.GetComponent<CardUI>();
                if(temp.Count == 0) cardUIScript.ToBagIndex = endIndex;
                else cardUIScript.ToBagIndex = -1;

                if (cardUIScript != null)
                {
                    cardUIScript.cardData = data;
                    cardUIScript.UpdateCardVisual();
                    cardUIScript.MoveTo(bagButton.GetComponent<RectTransform>().anchoredPosition, true, false, true);
                }
            }
        }
    }

    private void Shuffle(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardData temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

}
