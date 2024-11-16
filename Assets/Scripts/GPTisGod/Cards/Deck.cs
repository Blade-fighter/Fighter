using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // 玩家完整的卡组（用于永久存储所有卡牌）
    public List<CardData> drawPile = new List<CardData>(); // 抽牌堆
    public List<CardData> discardPile = new List<CardData>(); // 弃牌堆
    public List<CardData> hand = new List<CardData>(); // 手牌
    public int maxHandSize = 4; // 初始手牌数量

    void Start()
    {
        StartBattle();
    }

    public void StartBattle()
    {
        // 初始化战斗中的抽牌堆，将所有卡牌复制到抽牌堆
        drawPile = new List<CardData>(allCards);
        Shuffle(drawPile);
        discardPile.Clear();
        hand.Clear();

        // 抽取初始手牌
        for (int i = 0; i < maxHandSize; i++)
        {
            DrawCard();
        }
    }

    public void DrawCard()
    {
        if (hand.Count >= maxHandSize)
        {
            return; // 已达到手牌上限
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

            // TODO: 更新UI以显示新抽到的卡牌
            UpdateHandUI();
        }
    }

    public void PlayCard(CardData card)
    {
        if (hand.Contains(card))
        {
            hand.Remove(card);
            discardPile.Add(card);
            DrawCard(); // 保持手牌数量

            // TODO: 更新UI以显示手牌和弃牌堆的变化
            UpdateHandUI();
            UpdateDiscardPileUI();
        }
    }

    public void AddCardToDeck(CardData card, bool isPermanent)
    {
        if (isPermanent)
        {
            allCards.Add(card); // 永久添加到完整卡组
        }
        else
        {
            drawPile.Add(card); // 临时添加，仅当前战斗生效
        }
    }

    public void RemoveCardFromDeck(CardData card, bool isPermanent)
    {
        if (isPermanent)
        {
            allCards.Remove(card); // 永久移除
        }
        else
        {
            drawPile.Remove(card); // 临时移除
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
        // TODO: 实现手牌的UI更新逻辑，确保新抽到的卡牌和打出的卡牌能够正确显示
    }

    private void UpdateDiscardPileUI()
    {
        // TODO: 实现弃牌堆的UI更新逻辑
    }
}
