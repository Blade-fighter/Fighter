using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.GraphicsBuffer;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target, Character attacker);

    // 应用普通打击技硬直效果
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {

        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness); // 防御硬直
        }
        else if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
        {
            target.SetState(CharacterState.Stunned, idleHardness); // 受击硬直
        }
        else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // 打康硬直
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // 确反硬直
        }
    }

    //浮空效果
    public void ApplyAirBorne(Character target, Character attacker, int defendingHardness, int time, float value, int first, int next, int max, MoveEffect moveEffect)//浮空效果
    {
        if (target.currentState == CharacterState.Defending || target.currentState == CharacterState.BlockedStunned)//防住了
        {
            target.SetState(CharacterState.BlockedStunned, defendingHardness);
            ApplyMove(target, attacker, moveEffect.targetMoveBack, moveEffect.targetMoveKe, 0, 0);
        }
        else if (target.launchValue <= max)//如果对方没超过上限
        {
            //此处缺少浮空的具体效果

            if (target.currentState == CharacterState.Idle || target.currentState == CharacterState.Stunned)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time);//普通效果
            }
            else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // 打康效果
            }
            else if (target.currentState == CharacterState.Recovery)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // 确反硬直
            }
            else if (target.currentState == CharacterState.Airborne)
            {
                target.launchValue += next;
                target.SetState(CharacterState.Airborne, time); // 确反硬直
            }
        }
        else
        {
            Debug.Log("招式没效果");
        }
    }
        //移动效果
        public void ApplyMove(Character target,Character attacker,float targetMoveBack, int targetMoveKe, float attackerMove, int attackerMoveKe)
    {
        if (target != null)
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
        }
        if (attacker != null)
        {
            //普通的给自己添加移动效果
            attacker.MoveOverTime(attackerMove, attackerMoveKe);
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
        ApplyMove(target, attacker,targetMoveBack,targetMoveKe,attackerMove,attackerMoveKe);
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
[CreateAssetMenu(fileName = "New AirBorne Effect", menuName = "Card Effects/AirBorne Effect")]
public class AirBorneEffect : CardEffect
{
    public int defendingHardness; // 防御时的硬直时间

    public int airborneTime;//增加的浮空时间
    public float airborneValue;//浮空上升距离

    public int launchFirst; //命中未浮空敌人的浮空量
    public int launchNext;//命中已浮空敌人的浮空量
    public int launchMax;//浮空上限，当敌人浮空值大于此值无法命中

    public MoveEffect targetMoveEffect;

    public override void Trigger(Character target, Character attacker)
    {
        ApplyAirBorne(target,attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect);
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
