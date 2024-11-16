using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // ��������Ŀ��飨�������ô洢���п��ƣ�
    public List<CardData> drawPile = new List<CardData>(); // ���ƶ�
    public List<CardData> discardPile = new List<CardData>(); // ���ƶ�
    public List<CardData> hand = new List<CardData>(); // ����
    public int maxHandSize = 4; // ��ʼ��������

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

        if (drawPile.Count == 0)
        {
            ReshuffleDiscardIntoDraw();
        }

        if (drawPile.Count > 0)
        {
            CardData drawnCard = drawPile[0];
            drawPile.RemoveAt(0);
            hand.Add(drawnCard);
            Debug.Log("Drew card: " + drawnCard.cardName);

            // TODO: ����UI����ʾ�³鵽�Ŀ���
            UpdateHandUI();
        }
    }

    public void PlayCard(CardData card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);
            DrawCard(); // ������������

            // TODO: ����UI����ʾ���ƺ����ƶѵı仯
            UpdateHandUI();
            UpdateDiscardPileUI();
        }
    }

    public void AddCardToDeck(CardData card, bool isPermanent)
    {
        if (isPermanent)
        {
            allCards.Add(card); // ������ӵ���������
        }
        else
        {
            drawPile.Add(card); // ��ʱ��ӣ�����ǰս����Ч
        }
    }

    public void RemoveCardFromDeck(CardData card, bool isPermanent)
    {
        if (isPermanent)
        {
            allCards.Remove(card); // �����Ƴ�
        }
        else
        {
            drawPile.Remove(card); // ��ʱ�Ƴ�
        }
    }

    private void ReshuffleDiscardIntoDraw()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle(drawPile);
        Debug.Log("Reshuffled discard pile into draw pile.");
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
        // TODO: ʵ�����Ƶ�UI�����߼���ȷ���³鵽�Ŀ��ƺʹ���Ŀ����ܹ���ȷ��ʾ
    }

    private void UpdateDiscardPileUI()
    {
        // TODO: ʵ�����ƶѵ�UI�����߼�
    }
}
