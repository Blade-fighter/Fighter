using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Fight/Hardness Effect")]
public class HardnessEffect : CardEffect//ӲֱЧ��
{
    public int idleHardness;     // �ܻ�״̬��Ӳֱʱ��
    public int defendingHardness; // ����ʱ��Ӳֱʱ��
    public int attackInterruptedHardness; // �����ʱ��Ӳֱʱ��
    public int recoveryPunishHardness; // ��ȷ��ʱ��Ӳֱʱ��
    public MoveEffect moveEffect;
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.AirborneAttacked|| target.currentState != CharacterState.Downed)//�޷����е�״̬
        {
            ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
            ApplyMove(target, attacker, moveEffect.targetMoveBack, moveEffect.targetMoveKe, 0, 0);
        }
    }
}