using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // 敌人完整的卡组（用于永久存储所有卡牌）
    public List<CardData> drawPile = new List<CardData>(); // 抽牌堆
    public List<CardData> discardPile = new List<CardData>(); // 弃牌堆
    public List<CardData> hand = new List<CardData>(); // 手牌
    public int maxHandSize = 1; // 初始手牌数量
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
        // 初始化战斗中的抽牌堆，将所有卡牌复制到抽牌堆
        drawPile = new List<CardData>(allCards);
        for (int i = 0; i != drawPile.Count; ++i)
        {
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
    public void ReshuffleDiscardToDraw()//弃牌堆洗回抽牌堆
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
