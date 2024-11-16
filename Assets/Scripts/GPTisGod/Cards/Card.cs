using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string name; // ��������
    public int startupKe; // ��������Ŀ���
    public int activeKe; // �����ж�����Ŀ���
    public int recoveryKe; // ��������Ŀ���
    public int damage; // �˺�ֵ
    public CardEffect effect; // ���Ƹ�����Ч��
    public float pushbackDistance; // ������������ʱ�ĺ��˾���
    public AttackCollider attackColliderPrefab; // ������ײ��Ԥ����

    // ���캯��
    public Card(string name, int startupKe, int activeKe, int recoveryKe, int damage, CardEffect effect, float pushbackDistance, AttackCollider attackColliderPrefab)
    {
        this.name = name;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.damage = damage;
        this.effect = effect;
        this.pushbackDistance = pushbackDistance;
        this.attackColliderPrefab = attackColliderPrefab;
    }

    public void Execute(Character attacker, Character target)
    {
        // ���ó��н׶�״̬
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // �ָ�ʱ��������ִ�ж���
        TimeManager.Instance.ResumeGame();

        // �ӳ� startupKe ����������ж��׶�
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            attacker.SetState(CharacterState.AttackingActive, activeKe);

            // ����������ײ��
            if (attackColliderPrefab != null)
            {
                attacker.CreateCollider(attackColliderPrefab);
            }
        }, attacker));

        // �ӳ� startupKe + activeKe ��������н׶�
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // ����Ƿ�����
            if (attacker.attackCollider.GetComponent<AttackCollider>().hit)
            {
                Debug.Log(target.gameObject);
                effect?.Trigger(target);
            }
        }, attacker));

        // �ӳ� startupKe + activeKe + recoveryKe ��ָ�Ϊ Idle ״̬����ͣʱ��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            TimeManager.Instance.PauseGame(); // ��һָ�Ϊ Idle ״̬����ͣ��Ϸ
        }, attacker));
    }
}
