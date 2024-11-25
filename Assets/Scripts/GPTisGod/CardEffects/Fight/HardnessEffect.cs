using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Fight/Hardness Effect")]
public class HardnessEffect : CardEffect//硬直效果
{
    public int idleHardness;     // 受击状态的硬直时间
    public int defendingHardness; // 防御时的硬直时间
    public int attackInterruptedHardness; // 被打断时的硬直时间
    public int recoveryPunishHardness; // 被确反时的硬直时间
    public MoveEffect moveEffect;
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.AirborneAttacked|| target.currentState != CharacterState.Downed)//无法命中的状态
        {
            ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
            ApplyMove(target, attacker, moveEffect.targetMoveBack, moveEffect.targetMoveKe, 0, 0);
        }
    }
}