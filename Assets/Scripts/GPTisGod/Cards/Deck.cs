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
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);

            // ����Ƿ�����κ���Ч����
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            while(!DrawCard(card.handIndex)){
                // ϴ��
            }

            // �����߼�ȷ����������
            // while (hand.Count < maxHandSize)
            // {
            //     DrawCard();
            // }
            // if (drawPile.Count == 0)
            // {
            //     ReshuffleDiscardIntoDraw(); // ���ƶ�Ϊ��ʱ�����ƶ�ϴ����ƶ�
            // }
        }
    }

    private void ReshuffleDiscardIntoDraw()//���ƶ�ϴ�س��ƶ�
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
        // TODO: ʵ�����ƶѵ� UI �����߼�
    }


}
