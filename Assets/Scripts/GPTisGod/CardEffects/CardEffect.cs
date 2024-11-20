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
        else if (target.currentState == CharacterState.Airborne|| target.currentState == CharacterState.Jumping)
        {
            //浮空
            Debug.Log("浮空技命中空中敌人");
            target.SetState(CharacterState.AirborneAttacked, idleHardness); // 命中空中敌人（浮空/跳）
            target.ApplyAirborneAttackedEffect(idleHardness);
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
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.AttackingStartup || target.currentState == CharacterState.AttackingActive)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // 打康效果
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.Recovery)
            {
                target.launchValue += first;
                target.SetState(CharacterState.Airborne, time); // 确反硬直
                target.ApplyAirborneEffect(value, time);
            }
            else if (target.currentState == CharacterState.Airborne)
            {
                target.launchValue += next;
                target.SetState(CharacterState.Airborne, time); // 确反硬直
                target.ApplyAirborneEffect(value, time);
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
            //普通的给自身添加移动效果
            attacker.MoveOverTime(attackerMove, attackerMoveKe);
        }
    }
}
