using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public bool showCard = true;
    public List<CardData> allCards = new List<CardData>(); // ��������Ŀ��飨�������ô洢���п��ƣ�
    public List<CardData> drawPile = new List<CardData>(); // ���ƶ�
    public List<CardData> discardPile = new List<CardData>(); // ���ƶ�
    public List<CardData> hand = new List<CardData>(); // ����
    public int maxHandSize = 1; // ��ʼ��������

    public GameObject cardUIPrefab; // ���� UI Ԥ����
    public Transform handPanel; // ������ʾ���Ƶ� UI ���
    public float maxCardSpacing = 100f; // ����֮��������
    public float maxHandWidth = 600f; // ���Ƶ����ռ�ÿ��
    public float leftSpace=-250f;//����������λ��

    public GameObject discard;
    public static Deck instance;

    public Button discardButton;
    public Button bagButton;
    public Button closeZoneButton;
    public bool InFresh = false; // ��ϴ��
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
        // ��ʼ��ս���еĳ��ƶѣ������п��Ƹ��Ƶ����ƶ�
        drawPile = new List<CardData>(allCards);
        for(int i = 0;i != drawPile.Count; ++i){
            drawPile[i] = Instantiate(allCards[i]);
        }
        Shuffle(drawPile);
        discardPile.Clear();
        hand.Clear();

        // ��ȡ��ʼ����
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
            return true; // �Ѵﵽ��������
        }

        if (drawPile.Count > 0)
        {
            CardData drawnCard = drawPile[0];
            drawnCard.handIndex = index;
            drawPile.RemoveAt(0);
            hand.Add(drawnCard);

            // �������� UI
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

        // ���㿨�Ƽ��
        float cardSpacing = Mathf.Min(maxCardSpacing, maxHandWidth / Mathf.Max(1, hand.Count));

        GameObject cardUI = Instantiate(cardUIPrefab, handPanel);
        RectTransform cardRect = cardUI.GetComponent<RectTransform>();
        cardRect.anchoredPosition = Src;

        CardUI cardUIScript = cardUI.GetComponent<CardUI>();
        Vector2 Target = new Vector2(leftSpace + data.handIndex* cardSpacing, -375); // ���ÿ��Ƶ�λ�ã���˳������
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

            // ����Ƿ�����κ���Ч����
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            if(!DrawCard(card.handIndex)){
                // ϴ��
                ReshuffleDiscardToDraw(card.handIndex);
            }
        }
    }

    private void ReshuffleDiscardToDraw(int endIndex)//���ƶ�ϴ�س��ƶ�
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
