using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // ���������Ŀ��飨�������ô洢���п��ƣ�
    public List<CardData> drawPile = new List<CardData>(); // ���ƶ�
    public List<CardData> discardPile = new List<CardData>(); // ���ƶ�
    public List<CardData> hand = new List<CardData>(); // ����
    public int maxHandSize = 1; // ��ʼ��������
    // Start is called before the first frame update
    void Start()
    {
        StartBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartBattle()
    {
        // ��ʼ��ս���еĳ��ƶѣ������п��Ƹ��Ƶ����ƶ�
        drawPile = new List<CardData>(allCards);
        for (int i = 0; i != drawPile.Count; ++i)
        {
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
            return true;
        }
        return false;
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
    public void ReshuffleDiscardToDraw()//���ƶ�ϴ�س��ƶ�
    {
        if (discardPile.Count > 0)
        {
            List<CardData> temp = new List<CardData>(discardPile);
            Shuffle(temp);
            while (temp.Count != 0)
            {
                CardData data = temp[temp.Count - 1];
                drawPile.Add(data);
                temp.RemoveAt(temp.Count - 1);
                discardPile.Remove(data);
            }
        }
    }
}
