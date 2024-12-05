using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyAI : MonoBehaviour
{
    public Character character; // 敌人角色的引用
    public Deck enemyDeck; // 敌人手中的卡组引用
    public Character player; // 玩家角色的引用
    float playerDistance = 10f;//双方距离
    [Header("AI Settings")]
    public float moveForwardValue = 100; // 前进的基础优先级值
    public float moveBackwardValue = 100; // 后退的基础优先级值


    public int moveDurationKe = 5;  // 移动所需的刻数
    public float moveDistance = 2.0f;  // 移动的距离

    public int initialWaitKe = 5; // 初始等待的刻数
    private bool isDecisionPending = false; // 是否有待决策


    private void Start()
    {
        character = GameObject.FindWithTag("Enemy").GetComponent<Character>();//查找敌人自己
        player = GameObject.FindWithTag("Player").GetComponent<Character>(); // 查找玩家角色
        enemyDeck = gameObject.GetComponent<Deck>();
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + initialWaitKe, MakeDecision, character)); // 等待初始刻数后做第一次决策
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(character.transform.position, player.transform.position);
        if (character.currentState == CharacterState.Idle && !isDecisionPending)
        {
            MakeDecision();
        }
    }


    float CaculateValue(CardData cardData)//总计算值
    {
        float value = 0;
        value += CaculateDistanceValue(cardData);
        value += CaculateSelfHPValue(cardData);
        value += CaculatePlayerHPValue(cardData);
        return value;
    }
    //根据双方距离计算值
    float CaculateDistanceValue(CardData cardData)
    {
        if (cardData.cardType == CardType.Attack || cardData.cardType == CardType.MultiHit || cardData.cardType == CardType.Launch)
        {
            if (playerDistance <= cardData.distance)//招式能打中
            {
                return 100 + cardData.distanceValue;
            }
        }
        else if (cardData.cardType == CardType.Defense)
        {
            if (playerDistance <= 5)
            {
                return 100 + cardData.distanceValue;
            }
        }
        else
        {
            return 0;
        }
        return 0;
    }

    //根据自身血量计算值
    float CaculateSelfHPValue(CardData cardData)
    {
        //血量越低使用概率越大
        return 100 + (1 - (character.currentHealth / character.maxHealth)) * cardData.enemyHPValue;
    }
    //根据玩家血量计算值
    float CaculatePlayerHPValue(CardData cardData)
    {
        //血量越低使用概率越大
        return 100 + (1 - (player.currentHealth / player.maxHealth)) * cardData.playerHPValue;
    }
    //根据玩家状态计算值,眩晕时还需要按照眩晕时间做决策




    //根据自身之前的状态计算值，比如被投的次数，被打的次数

    private void MakeDecision()
    {
        isDecisionPending = true; // 标记正在做决策

        List<CardData> availableCards = enemyDeck.hand;
        List<(CardData, float)> actionPriorities = new List<(CardData, float)>();

        // 计算每张手牌的优先级值
        foreach (var card in availableCards)
        {
            float priority = CaculateValue(card);
            actionPriorities.Add((card, priority));
        }

        // 计算前进和后退的优先级值
        float forwardPriority = CalculateMoveForwardValue();
        float backwardPriority = CalculateMoveBackwardValue();

        actionPriorities.Add((null, forwardPriority)); // 使用 null 代表前进动作
        actionPriorities.Add((null, backwardPriority)); // 使用 null 代表后退动作

        // 随机选择一个行动，基于其优先级值
        (CardData chosenCard, float _) = ChooseAction(actionPriorities);

        ExecuteAction(chosenCard);
    }
    private float CalculateMoveForwardValue()
    {
        // 计算前进的优先级值，这里可以结合敌人和玩家的距离以及当前状态
        float priority = 100;
        float distance = Vector3.Distance(character.transform.position, player.transform.position);
        priority += distance > 5.0f ? moveForwardValue : -moveForwardValue; // 距离大于 5 时前进更有价值
        return priority;
    }

    private float CalculateMoveBackwardValue()
    {
        // 计算后退的优先级值，这里可以结合敌人和玩家的状态以及距离
        float priority = 100;
        float distance = Vector3.Distance(character.transform.position, player.transform.position);
        priority += distance < 5 ? moveBackwardValue : -moveBackwardValue; // 假设距离太近时后退更有价值
        return priority;
    }

    private (CardData, float) ChooseAction(List<(CardData, float)> actionPriorities)
    {
        float totalWeight = 0f;
        foreach (var (_, priority) in actionPriorities)
        {
            totalWeight += priority;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulativeWeight = 0f;

        foreach (var (card, priority) in actionPriorities)
        {
            cumulativeWeight += priority;
            if (randomValue <= cumulativeWeight)
            {
                return (card, priority);
            }
        }

        return (null, 0f); // 理论上不应该到达这里
    }

    private void ExecuteAction(CardData chosenCard)
    {
        if (chosenCard == null)
        {
            // 执行移动逻辑
            if (CalculateMoveForwardValue() > CalculateMoveBackwardValue())
            {
                character.MoveOverTime(moveDistance, moveDurationKe); // 前进
                character.SetState(CharacterState.MovingFront, moveDurationKe);
            }
            else
            {
                character.MoveOverTime(-moveDistance, moveDurationKe); // 后退
                character.SetState(CharacterState.MovingBack, moveDurationKe);
            }
        }
        else
        {
            // 执行卡牌效果
            Card card = new Card(chosenCard.cardName, chosenCard.cardType, chosenCard.cardDescription, chosenCard.cardImage, chosenCard.startupKe, chosenCard.activeKe, chosenCard.recoveryKe, chosenCard.collider, chosenCard.startEffect, chosenCard.hitEffect, chosenCard.multiHitData);
            // 执行卡牌逻辑
            card.Execute(character, player);
            //Debug.Log("敌人AI成功行动");
        }

        isDecisionPending = false; // 决策完成，标记为无待决策
    }

}
