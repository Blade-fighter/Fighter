using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Move Effect", menuName = "Card Effects/Fight/Move Effect")]
public class MoveEffect : CardEffect//移动效果
{
    [Header("纯移动技能把这个设置成true")]
    public bool setState=false;//纯移动技能把这个设置成true
    public float targetMoveBack;//敌方后退距离
    public int targetMoveKe;//敌方后退时间刻
    public float attackerMove;//我方前进距离
    public int attackerMoveKe;//我方前进时间刻
    public override void Trigger(Character target, Character attacker)
    {
        if (setState)
        {
            attacker.SetState(CharacterState.MovingFront,attackerMoveKe);
        }
        ApplyMove(target, attacker, targetMoveBack, targetMoveKe, attackerMove, attackerMoveKe);
    }
}
