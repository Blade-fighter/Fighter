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

    [Header("�˺����Ʒ�������")]
    public float damage;
    public float defenseDecrease;
    public float superIncrease;
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.AirborneAttacked|| target.currentState != CharacterState.Downed)//�޷����е�״̬
        {
            ApplyHardness(target, attacker,idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness,damage,defenseDecrease,superIncrease);
            //����ƶ���ʵҲ����д��applyhardness���棬����ÿ����������д�Ƚ��鷳���ͷ�������
            ApplyMove(target, attacker, moveEffect.targetMoveBack, moveEffect.targetMoveKe, 0, 0);
        }
    }
}