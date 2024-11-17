using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // ��������Ŀ��飨�������ô洢���п��ƣ�
    public List<CardData> drawPile = new List<CardData>(); // ���ƶ�
    public List<CardData> discardPile = new List<CardData>(); // ���ƶ�
    public List<CardData> hand = new List<CardData>(); // ����
    public int maxHandSize = 4; // ��ʼ��������

    public GameObject cardUIPrefab; // ���� UI Ԥ����
    public Transform handPanel; // ������ʾ���Ƶ� UI ���
    public float maxCardSpacing = 100f; // ����֮��������
    public float maxHandWidth = 600f; // ���Ƶ����ռ�ÿ��
    public float leftSpace=0f;//����������λ��

    void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        // ��ʼ��ս���еĳ��ƶѣ������п��Ƹ��Ƶ����ƶ�
        drawPile = new List<CardData>(allCards);
        Shuffle(drawPile);
        discardPile.Clear();
        hand.Clear();

        // ��ȡ��ʼ����
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (hand.Count >= maxHandSize)
        {
            return; // �Ѵﵽ��������
        }

        if (drawPile.Count > 0)
        {
            CardData drawnCard = drawPile[0];
            hand.Insert(0, drawnCard); // ���¿��Ʋ������Ƶ������
            drawPile.RemoveAt(0);
            Debug.Log("Drew card: " + drawnCard.cardName);

            // �������� UI
            UpdateHandUI();
        }
    }

    public void PlayCard(CardData card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);
            UpdateHandUI();

            // ����Ƿ�����κ���Ч����
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            // �����߼�ȷ����������
            while (hand.Count < maxHandSize)
            {
                DrawCard();
            }
            if (drawPile.Count == 0)
            {
                ReshuffleDiscardIntoDraw(); // ���ƶ�Ϊ��ʱ�����ƶ�ϴ����ƶ�
            }
        }
    }

    private void ReshuffleDiscardIntoDraw()//���ƶ�ϴ�س��ƶ�
    {
        if (discardPile.Count > 0)
        {
            drawPile.AddRange(discardPile);
            discardPile.Clear();
            Shuffle(drawPile);
            Debug.Log("Reshuffled discard pile into draw pile.");
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

    private void UpdateHandUI()
    {
        // ��յ�ǰ���Ƶ� UI ��ʾ
        foreach (Transform child in handPanel)
        {
            Destroy(child.gameObject);
        }

        // ���㿨�Ƽ��
        float cardSpacing = Mathf.Min(maxCardSpacing, maxHandWidth / Mathf.Max(1, hand.Count));

        // Ϊÿ����������һ���µ� UI Ԫ�أ�����˳������
        for (int i = 0; i < hand.Count; i++)
        {
            CardData card = hand[i];
            GameObject cardUI = Instantiate(cardUIPrefab, handPanel);
            RectTransform cardRect = cardUI.GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector2(leftSpace+i * cardSpacing, 0); // ���ÿ��Ƶ�λ�ã���˳������

            // ���ÿ��ƵĲ㼶��ȷ�������ɵĿ�����ǰһ��֮��
            cardUI.transform.SetSiblingIndex(i);

            CardUI cardUIScript = cardUI.GetComponent<CardUI>();
            if (cardUIScript != null)
            {
                cardUIScript.cardData = card;
                cardUIScript.UpdateCardVisual();
            }
            else
            {
                Debug.LogWarning("CardUI script is missing on the card UI prefab.");
            }
        }
    }

    private void UpdateDiscardPileUI()
    {
        // TODO: ʵ�����ƶѵ� UI �����߼�
    }
}
