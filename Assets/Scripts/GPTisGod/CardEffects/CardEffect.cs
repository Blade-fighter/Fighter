using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target, Character attacker);

    // Ӧ����ͨ�����ӲֱЧ��
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {

        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness); // ����Ӳֱ
        }
        else if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
        {
            target.SetState(CharacterState.Stunned, idleHardness); // �ܻ�Ӳֱ
        }
        else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // ��Ӳֱ
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // ȷ��Ӳֱ
        }
    }

    //����Ч��
    public void ApplyAirBorne(Character target, Character attacker, int defendingHardness, int time, float value, int first, int next, int max, MoveEffect moveEffect)//����Ч��
    {
        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)//��ס��
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness);
            ApplyMove(target, attacker, moveEffect.targetMoveBack, moveEffect.targetMoveKe, 0, 0);
        }
        else if (target.launchValue <= max)//����Է�û��������
        {
            //�˴�ȱ�ٸ��յľ���Ч��

            if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time);//��ͨЧ��
            }
            else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // ��Ч��
            }
            else if (target.currentState == CharacterState.Recovery)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // ȷ��Ӳֱ
            }
            else if (target.currentState == CharacterState.Airborne)
            {
                target.launchValue += next;
                target.SetState(CharacterState.Airborne, time); // ȷ��Ӳֱ
            }
        }
        else
        {
            Debug.Log("��ʽûЧ��");
        }
    }
        //�ƶ�Ч��
        public void ApplyMove(Character target,Character attacker,float targetMoveBack, int targetMoveKe, float attackerMove, int attackerMoveKe)
    {
        if (target != null)
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
        }
        if (attacker != null)
        {
            //��ͨ�ĸ��Լ�����ƶ�Ч��
            attacker.MoveOverTime(attackerMove, attackerMoveKe);
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
        ApplyMove(target, attacker,targetMoveBack,targetMoveKe,attackerMove,attackerMoveKe);
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
[CreateAssetMenu(fileName = "New AirBorne Effect", menuName = "Card Effects/AirBorne Effect")]
public class AirBorneEffect : CardEffect
{
    public int defendingHardness; // ����ʱ��Ӳֱʱ��

    public int airborneTime;//���ӵĸ���ʱ��
    public float airborneValue;//������������

    public int launchFirst; //����δ���յ��˵ĸ�����
    public int launchNext;//�����Ѹ��յ��˵ĸ�����
    public int launchMax;//�������ޣ������˸���ֵ���ڴ�ֵ�޷�����

    public MoveEffect targetMoveEffect;

    public override void Trigger(Character target, Character attacker)
    {
        ApplyAirBorne(target,attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect);
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
