using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Effect", menuName = "Card Effects/Fight/Move Effect")]
public class MoveEffect : CardEffect//移动效果
{
    public float targetMoveBack;//敌方后退距离
    public int targetMoveKe;//敌方后退时间刻
    public float attackerMove;//我方前进距离
    public int attackerMoveKe;//我方前进时间刻
    public override void Trigger(Character target, Character attacker)
    {
        ApplyMove(target, attacker, targetMoveBack, targetMoveKe, attackerMove, attackerMoveKe);
    }
}
