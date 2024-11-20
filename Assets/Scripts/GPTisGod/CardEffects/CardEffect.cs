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
        else if (target.currentState == CharacterState.Airborne|| target.currentState == CharacterState.Jumping)
        {
            //����
            Debug.Log("���ռ����п��е���");
            target.SetState(CharacterState.AirborneAttacked, idleHardness); // ���п��е��ˣ�����/����
            target.ApplyAirborneAttackedEffect(idleHardness);
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
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // ��Ч��
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.Recovery)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // ȷ��Ӳֱ
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.Airborne)
            {
                target.launchValue += next;
                target.SetState(CharacterState.Airborne, time); // ȷ��Ӳֱ
                target.ApplyAirborneEffect(value, time);
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
            //��ͨ�ĸ���������ƶ�Ч��
            attacker.MoveOverTime(attackerMove, attackerMoveKe);
        }
    }
}
