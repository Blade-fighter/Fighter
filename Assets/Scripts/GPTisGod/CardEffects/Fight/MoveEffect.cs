using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Effect", menuName = "Card Effects/Fight/Move Effect")]
public class MoveEffect : CardEffect//�ƶ�Ч��
{
    public float targetMoveBack;//�з����˾���
    public int targetMoveKe;//�з�����ʱ���
    public float attackerMove;//�ҷ�ǰ������
    public int attackerMoveKe;//�ҷ�ǰ��ʱ���
    public override void Trigger(Character target, Character attacker)
    {
        ApplyMove(target, attacker, targetMoveBack, targetMoveKe, attackerMove, attackerMoveKe);
    }
}
