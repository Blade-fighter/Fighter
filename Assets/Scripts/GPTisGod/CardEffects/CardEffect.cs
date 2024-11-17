using JetBrains.Annotations;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target, Character attacker);

    // 应用硬直效果
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {

        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness); // 防御硬直
        }
        else if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
        {
            Debug.Log("确实是设置了");
            target.SetState(CharacterState.Stunned, idleHardness); // 受击硬直
        }
        else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // 被打断硬直
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // 确反硬直
        }
    }
}
[CreateAssetMenu(fileName = "New Move Effect", menuName = "Card Effects/Move Effect")]
public class MoveEffect : CardEffect//移动效果
{
    public float targetMoveBack;//敌方后退距离
    public int targetMoveKe;//敌方后退时间刻
    public float attackerMove;//我方前进距离
    public int attackerMoveKe;//我方前进时间刻
    public override void Trigger(Character target, Character attacker)
    {
        //击退对方，对方在版边改为自己后退
        if (target.IsAtBoundary())
        {
            attacker.MoveBack(targetMoveBack, targetMoveKe);
        }
        else
        {
            target.MoveBack(targetMoveBack, targetMoveKe);
        }
        //普通的给自己添加移动效果
        attacker.MoveOverTime(attackerMove, attackerMoveKe);
    }
}


[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Hardness Effect")]
public class HardnessEffect : CardEffect//硬直效果
{
    public int idleHardness;     // 受击状态的硬直时间
    public int defendingHardness; // 防御时的硬直时间
    public int attackInterruptedHardness; // 被打断时的硬直时间
    public int recoveryPunishHardness; // 被确反时的硬直时间
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.Airborne || target.currentState != CharacterState.Jumping)//空中情况特殊考虑
        {
            ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
        }
        else if (true)
        {
            //浮空

        }
        else
        {
            //跳跃攻击中
        }
    }
}

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Heal Effect")]
public class HealEffect : CardEffect
{
    public int healAmount; // 治疗量

    public override void Trigger(Character target, Character attacker)
    {
        target.Heal(healAmount);
        Debug.Log(target.name + " is healed for " + healAmount + " health.");
    }
}
