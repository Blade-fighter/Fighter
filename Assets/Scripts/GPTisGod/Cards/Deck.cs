using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>(); // 玩家完整的卡组（用于永久存储所有卡牌）
    public List<CardData> drawPile = new List<CardData>(); // 抽牌堆
    public List<CardData> discardPile = new List<CardData>(); // 弃牌堆
    public List<CardData> hand = new List<CardData>(); // 手牌
    public int maxHandSize = 4; // 初始手牌数量

    public GameObject cardUIPrefab; // 卡牌 UI 预制体
    public Transform handPanel; // 用于显示手牌的 UI 面板
    public float maxCardSpacing = 100f; // 卡牌之间的最大间隔
    public float maxHandWidth = 600f; // 手牌的最大占用宽度
    public float leftSpace=0f;//手牌最左侧的位置

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

        if (drawPile.Count > 0)
        {
            CardData drawnCard = drawPile[0];
            hand.Insert(0, drawnCard); // 将新卡牌插入手牌的最左边
            drawPile.RemoveAt(0);
            Debug.Log("Drew card: " + drawnCard.cardName);

            // 更新手牌 UI
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

            // 检查是否存在任何无效引用
            if (card == null)
            {
                Debug.LogWarning("Card reference is null, might have been destroyed unexpectedly.");
            }

            // 抽牌逻辑确保手牌上限
            while (hand.Count < maxHandSize)
            {
                DrawCard();
            }
            if (drawPile.Count == 0)
            {
                ReshuffleDiscardIntoDraw(); // 抽牌堆为空时将弃牌堆洗入抽牌堆
            }
        }
    }

    private void ReshuffleDiscardIntoDraw()//弃牌堆洗回抽牌堆
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
        // 清空当前手牌的 UI 显示
        foreach (Transform child in handPanel)
        {
            Destroy(child.gameObject);
        }

        // 计算卡牌间距
        float cardSpacing = Mathf.Min(maxCardSpacing, maxHandWidth / Mathf.Max(1, hand.Count));

        // 为每张手牌生成一个新的 UI 元素，并按顺序排列
        for (int i = 0; i < hand.Count; i++)
        {
            CardData card = hand[i];
            GameObject cardUI = Instantiate(cardUIPrefab, handPanel);
            RectTransform cardRect = cardUI.GetComponent<RectTransform>();
            cardRect.anchoredPosition = new Vector2(leftSpace+i * cardSpacing, 0); // 设置卡牌的位置，按顺序排列

            // 设置卡牌的层级，确保后生成的卡牌在前一张之上
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
        // TODO: 实现弃牌堆的 UI 更新逻辑
    }
}
