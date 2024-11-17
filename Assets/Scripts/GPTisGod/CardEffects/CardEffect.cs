using JetBrains.Annotations;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target, Character attacker);

    // Ӧ��ӲֱЧ��
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {

        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness); // ����Ӳֱ
        }
        else if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
        {
            Debug.Log("ȷʵ��������");
            target.SetState(CharacterState.Stunned, idleHardness); // �ܻ�Ӳֱ
        }
        else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // �����Ӳֱ
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // ȷ��Ӳֱ
        }
    }
}
[CreateAssetMenu(fileName = "New Move Effect", menuName = "Card Effects/Move Effect")]
public class MoveEffect : CardEffect//�ƶ�Ч��
{
    public float targetMoveBack;//�з����˾���
    public int targetMoveKe;//�з�����ʱ���
    public float attackerMove;//�ҷ�ǰ������
    public int attackerMoveKe;//�ҷ�ǰ��ʱ���
    public override void Trigger(Character target, Character attacker)
    {
        //���˶Է����Է��ڰ�߸�Ϊ�Լ�����
        if (target.IsAtBoundary())
        {
            attacker.MoveBack(targetMoveBack, targetMoveKe);
        }
        else
        {
            target.MoveBack(targetMoveBack, targetMoveKe);
        }
        //��ͨ�ĸ��Լ�����ƶ�Ч��
        attacker.MoveOverTime(attackerMove, attackerMoveKe);
    }
}


[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Hardness Effect")]
public class HardnessEffect : CardEffect//ӲֱЧ��
{
    public int idleHardness;     // �ܻ�״̬��Ӳֱʱ��
    public int defendingHardness; // ����ʱ��Ӳֱʱ��
    public int attackInterruptedHardness; // �����ʱ��Ӳֱʱ��
    public int recoveryPunishHardness; // ��ȷ��ʱ��Ӳֱʱ��
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.Airborne || target.currentState != CharacterState.Jumping)//����������⿼��
        {
            ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
        }
        else if (true)
        {
            //����

        }
        else
        {
            //��Ծ������
        }
    }
}

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Heal Effect")]
public class HealEffect : CardEffect
{
    public int healAmount; // ������

    public override void Trigger(Character target, Character attacker)
    {
        target.Heal(healAmount);
        Debug.Log(target.name + " is healed for " + healAmount + " health.");
    }
}
