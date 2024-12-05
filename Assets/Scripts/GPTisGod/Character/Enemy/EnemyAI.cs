using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyAI : MonoBehaviour
{
    public Character character; // ���˽�ɫ������
    public Deck enemyDeck; // �������еĿ�������
    public Character player; // ��ҽ�ɫ������
    float playerDistance = 10f;//˫������
    [Header("AI Settings")]
    public float moveForwardValue = 100; // ǰ���Ļ������ȼ�ֵ
    public float moveBackwardValue = 100; // ���˵Ļ������ȼ�ֵ


    public int moveDurationKe = 5;  // �ƶ�����Ŀ���
    public float moveDistance = 2.0f;  // �ƶ��ľ���

    public int initialWaitKe = 5; // ��ʼ�ȴ��Ŀ���
    private bool isDecisionPending = false; // �Ƿ��д�����


    private void Start()
    {
        character = GameObject.FindWithTag("Enemy").GetComponent<Character>();//���ҵ����Լ�
        player = GameObject.FindWithTag("Player").GetComponent<Character>(); // ������ҽ�ɫ
        enemyDeck = gameObject.GetComponent<Deck>();
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + initialWaitKe, MakeDecision, character)); // �ȴ���ʼ����������һ�ξ���
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(character.transform.position, player.transform.position);
        if (character.currentState == CharacterState.Idle && !isDecisionPending)
        {
            MakeDecision();
        }
    }


    float CaculateValue(CardData cardData)//�ܼ���ֵ
    {
        float value = 0;
        value += CaculateDistanceValue(cardData);
        value += CaculateSelfHPValue(cardData);
        value += CaculatePlayerHPValue(cardData);
        return value;
    }
    //����˫���������ֵ
    float CaculateDistanceValue(CardData cardData)
    {
        if (cardData.cardType == CardType.Attack || cardData.cardType == CardType.MultiHit || cardData.cardType == CardType.Launch)
        {
            if (playerDistance <= cardData.distance)//��ʽ�ܴ���
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

    //��������Ѫ������ֵ
    float CaculateSelfHPValue(CardData cardData)
    {
        //Ѫ��Խ��ʹ�ø���Խ��
        return 100 + (1 - (character.currentHealth / character.maxHealth)) * cardData.enemyHPValue;
    }
    //�������Ѫ������ֵ
    float CaculatePlayerHPValue(CardData cardData)
    {
        //Ѫ��Խ��ʹ�ø���Խ��
        return 100 + (1 - (player.currentHealth / player.maxHealth)) * cardData.playerHPValue;
    }
    //�������״̬����ֵ,ѣ��ʱ����Ҫ����ѣ��ʱ��������




    //��������֮ǰ��״̬����ֵ�����类Ͷ�Ĵ���������Ĵ���

    private void MakeDecision()
    {
        isDecisionPending = true; // �������������

        List<CardData> availableCards = enemyDeck.hand;
        List<(CardData, float)> actionPriorities = new List<(CardData, float)>();

        // ����ÿ�����Ƶ����ȼ�ֵ
        foreach (var card in availableCards)
        {
            float priority = CaculateValue(card);
            actionPriorities.Add((card, priority));
        }

        // ����ǰ���ͺ��˵����ȼ�ֵ
        float forwardPriority = CalculateMoveForwardValue();
        float backwardPriority = CalculateMoveBackwardValue();

        actionPriorities.Add((null, forwardPriority)); // ʹ�� null ����ǰ������
        actionPriorities.Add((null, backwardPriority)); // ʹ�� null ������˶���

        // ���ѡ��һ���ж������������ȼ�ֵ
        (CardData chosenCard, float _) = ChooseAction(actionPriorities);

        ExecuteAction(chosenCard);
    }
    private float CalculateMoveForwardValue()
    {
        // ����ǰ�������ȼ�ֵ��������Խ�ϵ��˺���ҵľ����Լ���ǰ״̬
        float priority = 100;
        float distance = Vector3.Distance(character.transform.position, player.transform.position);
        priority += distance > 5.0f ? moveForwardValue : -moveForwardValue; // ������� 5 ʱǰ�����м�ֵ
        return priority;
    }

    private float CalculateMoveBackwardValue()
    {
        // ������˵����ȼ�ֵ��������Խ�ϵ��˺���ҵ�״̬�Լ�����
        float priority = 100;
        float distance = Vector3.Distance(character.transform.position, player.transform.position);
        priority += distance < 5 ? moveBackwardValue : -moveBackwardValue; // �������̫��ʱ���˸��м�ֵ
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

        return (null, 0f); // �����ϲ�Ӧ�õ�������
    }

    private void ExecuteAction(CardData chosenCard)
    {
        if (chosenCard == null)
        {
            // ִ���ƶ��߼�
            if (CalculateMoveForwardValue() > CalculateMoveBackwardValue())
            {
                character.MoveOverTime(moveDistance, moveDurationKe); // ǰ��
                character.SetState(CharacterState.MovingFront, moveDurationKe);
            }
            else
            {
                character.MoveOverTime(-moveDistance, moveDurationKe); // ����
                character.SetState(CharacterState.MovingBack, moveDurationKe);
            }
        }
        else
        {
            // ִ�п���Ч��
            Card card = new Card(chosenCard.cardName, chosenCard.cardType, chosenCard.cardDescription, chosenCard.cardImage, chosenCard.startupKe, chosenCard.activeKe, chosenCard.recoveryKe, chosenCard.collider, chosenCard.startEffect, chosenCard.hitEffect, chosenCard.multiHitData);
            // ִ�п����߼�
            card.Execute(character, player);
            //Debug.Log("����AI�ɹ��ж�");
        }

        isDecisionPending = false; // ������ɣ����Ϊ�޴�����
    }

}
