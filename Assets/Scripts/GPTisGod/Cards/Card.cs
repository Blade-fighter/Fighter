using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Card
{
    public string name; // ��������
    public CardType cardType;      //��������
    public string cardDescription;//��������
    public Sprite cardImage;     // ����ͼ��
    public float damage; // �˺�ֵ
    public float defenseDecrease;//�Ʒ�ֵ
    public float superIncrease;//����ɱ������
    public int startupKe; // ��������Ŀ���
    public int activeKe; // �����ж�����Ŀ���
    public int recoveryKe; // ��������Ŀ���,�����ʽֻ���������
    public CardEffect[] startEffect; // ���ƴ��ʱ������Ч��
    public CardEffect[] hitEffect; // ��������ʱ������Ч��
    public AttackCollider attackColliderPrefab; // ������ײ��Ԥ����

    public GameObject activeCollider; // �������õ�ǰ��������ײ��
    static int executeTimes = 0;

    public List<HitData> multiHitData; // ��ι���������
    // ���캯��
    public Card(string name, CardType cardType, string cardDescription, Sprite cardImage,int startupKe, int activeKe, int recoveryKe, AttackCollider attackColliderPrefab, CardEffect[] startEffect, CardEffect[] hitEffect,List<HitData> multiHitData)
    {
        this.name = name;
        this.cardType = cardType;
        this.cardDescription = cardDescription;
        this.cardImage = cardImage;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.attackColliderPrefab = attackColliderPrefab;
        this.startEffect = startEffect;
        this.hitEffect = hitEffect;
        this.multiHitData = multiHitData;
    }

    public void Execute(Character attacker, Character target)
    {
        if(attacker.gameObject.tag == "Player"){
            executeTimes += 1;
            TreasureContext context = new TreasureContext();
            context.CardCount = executeTimes;
            context.character = attacker;
            TreasureManager.Instance.ApplyTreasure(context, EffectTime.Card);
        }

        switch (cardType)
        {
            case CardType.Defense://����
                ExecuteDefense(attacker);
                break;
            case CardType.Attack://��ͨ�����
                ExecuteAttack(attacker, target);
                break;
            case CardType.Launch://���ռ�
                ExecuteAttack(attacker, target);
                break;
            case CardType.MultiHit:
                ExecuteMultiHitAttack(attacker, target);
                break;
            case CardType.Move:
                ExecuteMove(attacker, target);
                break;
            // �������͵Ŀ��ƿ���������������
            default:
                Debug.LogWarning("δ֪�Ŀ�������: " + cardType);
                break;
        }
    }

    private void ExecuteDefense(Character character)//�����Ĵ���
    {
        // ���ý�ɫΪ����״̬
        character.SetState(CharacterState.Defending, startupKe);

        // �ָ�ʱ����������ִ�ж���
        if(character.tag == "Player")
        {
            TimeManager.Instance.ResumeGame(); 
        }

        // �ӳ� startupKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            character.SetState(CharacterState.Idle, 0);
            if (character.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }
            character.animator.SetInteger("AttackIndex", 0);//������ʽindex
            // ֪ͨ CardUI ����Ч�������
            CardUI.CardEffectComplete();
        }, character));
    }


    public void ExecuteAttack(Character attacker, Character target)
    {
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // �ָ�ʱ��������ִ�ж���
        if (attacker.tag == "Player")
        {
            TimeManager.Instance.ResumeGame();
        }
        // ��ʼЧ������
        foreach (CardEffect effect in startEffect)
        {
            effect?.Trigger(target, attacker);
        }

        // �ӳ� startupKe ����������ж��׶�
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            attacker.SetState(CharacterState.AttackingActive, activeKe);

            // ����������ײ��
            if (attackColliderPrefab != null)
            {
                activeCollider = attacker.CreateCollider(attackColliderPrefab);
            }

        }, attacker));

        // ��ÿһ�̵������ж��н��м��
        for (int i = 0; i < activeKe; i++)
        {
            int keToExecute = TimeManager.Instance.currentKe + startupKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
                {
                    //Debug.Log("��ʽ������: " + target.gameObject.name);
                    foreach (CardEffect effect in hitEffect)
                    {
                        effect?.Trigger(target, attacker);
                    }
                    // ���ٹ�����ײ�壬�����δ���
                    GameObject.Destroy(activeCollider);
                    activeCollider = null;
                }
            }, attacker));
        }

        // �ӳ� startupKe + activeKe ��������н׶�
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // �����ײ��δ���٣��������н׶�����
            if (activeCollider != null)
            {   
                //����ʱ���ж�һ��
                if (activeCollider.GetComponent<AttackCollider>().hit)
                {
                    //Debug.Log("��ʽ������: " + target.gameObject.name);
                    foreach (CardEffect effect in hitEffect)
                    {
                        effect?.Trigger(target, attacker);
                    }
                    // ���ٹ�����ײ�壬�����δ���
                    GameObject.Destroy(activeCollider);
                    activeCollider = null;
                }
                //Debug.Log(TimeManager.Instance.currentKe + "��ʱ��" + activeCollider + "������");
                GameObject.Destroy(activeCollider);
            }
        }, attacker));

        // �ӳ� startupKe + activeKe + recoveryKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }
            attacker.animator.SetInteger("AttackIndex", 0);//������ʽindex
            // ֪ͨ CardUI ����Ч�������
            CardUI.CardEffectComplete();
        }, attacker));
    }
    private void ExecuteMultiHitAttack(Character attacker, Character target)
    {
        int currentHitIndex = 0;

        void ExecuteNextHit()
        {
            if (currentHitIndex >= multiHitData.Count)
            {
                // ���й���������ɣ��������н׶�
                attacker.SetState(CharacterState.Recovery, recoveryKe);
                // �ӳ� startupKe + activeKe + recoveryKe ��ָ�Ϊ Idle ״̬����ͣʱ��
                ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
                {
                    attacker.SetState(CharacterState.Idle, 0);
                    if (attacker.tag == "Player")
                    {
                        TimeManager.Instance.PauseGame();
                    }
                    attacker.animator.SetInteger("AttackIndex", 0);//������ʽindex
                    // ֪ͨ CardUI ����Ч�������
                    CardUI.CardEffectComplete();
                }, attacker));
                return;
            }

            HitData currentHit = multiHitData[currentHitIndex];
            attacker.SetState(CharacterState.AttackingStartup, currentHit.startupKe);
            // �ָ�ʱ��������ִ�ж���
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.ResumeGame();
            }

            foreach (CardEffect effect in currentHit.startEffects)//ÿ�ε���ʼЧ��
            {
                effect?.Trigger(target, attacker);
            }
            // ������ײ��
            GameObject activeCollider = attacker.CreateCollider(currentHit.attackColliderPrefab.GetComponent<AttackCollider>());

            // �ӳ� startupKe ����������ж��׶�
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + currentHit.startupKe, () =>
            {
                attacker.SetState(CharacterState.AttackingActive, currentHit.activeKe);

                // �����ж��߼�
                ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + currentHit.activeKe, () =>
                {
                    if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
                    {
                        //Debug.Log("��" + (currentHitIndex + 1) + "�ι�������: " + target.gameObject.name);
                        foreach (CardEffect effect in currentHit.hitEffects)
                        {
                            effect?.Trigger(target, attacker);
                        }
                    }

                    // ���ٵ�ǰ��ײ��
                    GameObject.Destroy(activeCollider);

                    // ������һ�ι���
                    currentHitIndex++;
                    ExecuteNextHit();
                }, attacker));
            }, attacker));
        }

        // ִ�е�һ�ι���
        ExecuteNextHit();
    }

    //���ƶ����룬������
    public void ExecuteMove(Character attacker, Character target)
    {
        // �ָ�ʱ��������ִ�ж���
        if (attacker.tag == "Player")
        {
            TimeManager.Instance.ResumeGame();
        }

        // ��ʼЧ������
        foreach (CardEffect effect in startEffect)
        {
            effect?.Trigger(target, attacker);
        }
        // �ӳ� startupKe + activeKe + recoveryKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }

            // ֪ͨ CardUI ����Ч�������
            CardUI.CardEffectComplete();
        }, attacker));
    }
}


