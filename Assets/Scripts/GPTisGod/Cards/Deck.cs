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

    void Awake(){        
        instance = this;
    }

    private void OnDiscardButtonClick(){
        Debug.Log("OnDiscardButtonClick");
    }

    private void OnBagButtonClick(){
        Debug.Log("OnBagButtonClick");
    }


    void Start()
    {
        discardButton.onClick.AddListener(OnDiscardButtonClick);
        bagButton.onClick.AddListener(OnBagButtonClick);
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
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);

            // 检查是否存在任何无效引用
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            while(!DrawCard(card.handIndex)){
                // 洗牌
            }

            // 抽牌逻辑确保手牌上限
            // while (hand.Count < maxHandSize)
            // {
            //     DrawCard();
            // }
            // if (drawPile.Count == 0)
            // {
            //     ReshuffleDiscardIntoDraw(); // 抽牌堆为空时将弃牌堆洗入抽牌堆
            // }
        }
    }

    private void ReshuffleDiscardIntoDraw()//弃牌堆洗回抽牌堆
    {
        if (discardPile.Count > 0)
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
            //Debug.Log("Reshuffled discard pile into draw pile.");
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

    private void UpdateDiscardPileUI()
    {
        // TODO: 实现弃牌堆的 UI 更新逻辑
    }


}
