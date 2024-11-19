using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string name; // ��������
    public CardType cardType;      //��������
    public string cardDescription;//��������
    public Sprite cardImage;     // ����ͼ��
    public int startupKe; // ��������Ŀ���
    public int activeKe; // �����ж�����Ŀ���
    public int recoveryKe; // ��������Ŀ���
    public int damage; // �˺�ֵ
    public CardEffect[] startEffect; // ���ƴ��ʱ������Ч��
    public CardEffect[] hitEffect; // ��������ʱ������Ч��
    public AttackCollider attackColliderPrefab; // ������ײ��Ԥ����

    private GameObject activeCollider; // �������õ�ǰ��������ײ��

    // ���캯��
    public Card(string name, CardType cardType, string cardDescription, Sprite cardImage, int startupKe, int activeKe, int recoveryKe, CardEffect[] startEffect, CardEffect[] hitEffect, AttackCollider attackColliderPrefab)
    {
        this.name = name;
        this.cardType = cardType;
        this.cardDescription = cardDescription;
        this.cardImage = cardImage;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.startEffect = startEffect;
        this.hitEffect = hitEffect;
        this.attackColliderPrefab = attackColliderPrefab;
    }

    public void Execute(Character attacker, Character target)
    {
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
        TimeManager.Instance.ResumeGame();

        // �ӳ� startupKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            character.SetState(CharacterState.Idle, 0);
            TimeManager.Instance.PauseGame(); // ��һָ�Ϊ Idle ״̬����ͣ��Ϸ

            // ֪ͨ CardUI ����Ч�������
            CardUI.CardEffectComplete();
        }, character));
    }

    private void ExecuteAttack(Character attacker, Character target)
    {
        // ԭ�еĹ����߼�
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // �ָ�ʱ��������ִ�ж���
        TimeManager.Instance.ResumeGame();

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

                attacker.StartCoroutine(ActiveKeHitDetection(attacker, target));
        }, attacker));

        // �ӳ� startupKe + activeKe ��������н׶�
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // �����ײ��δ���٣��������н׶�����
            if (activeCollider != null)
            {
                //Debug.Log(TimeManager.Instance.currentKe + "��ʱ��" + activeCollider + "������");
                //GameObject.Destroy(activeCollider);
            }
        }, attacker));

        // �ӳ� startupKe + activeKe + recoveryKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            TimeManager.Instance.PauseGame(); // ��һָ�Ϊ Idle ״̬����ͣ��Ϸ

            // ֪ͨ CardUI ����Ч�������
            CardUI.CardEffectComplete();
        }, attacker));
    }

    private IEnumerator ActiveKeHitDetection(Character attacker, Character target)
    {
        for (int i = 0; i < activeKe; i++)
        {
            yield return Time.deltaTime;
            //Debug.Log(activeCollider + "��hit��" + activeCollider.GetComponent<AttackCollider>().hit);
            if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
            {
                Debug.Log("�ڵ�" + (TimeManager.Instance.currentKe) + "�̣���ʽ������" + target.gameObject);
                foreach (CardEffect effect in hitEffect)
                {
                    effect?.Trigger(target, attacker);
                }
                // ���ٹ�����ײ�壬�����δ���
                GameObject.Destroy(activeCollider);
                activeCollider = null;
                yield break; // ����Э�̣���Ϊ�Ѿ�����Ŀ��
            }
            yield return new WaitForSeconds(TimeManager.Instance.keDuration);
        }
    }
}
