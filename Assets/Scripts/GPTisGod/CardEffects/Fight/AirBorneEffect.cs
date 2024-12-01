using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AirBorne Effect", menuName = "Card Effects/Fight/AirBorne Effect")]
public class AirBorneEffect : CardEffect
{
    public int defendingHardness; // 防御时的硬直时间

    public int airborneTime;//增加的浮空时间
    public float airborneValue;//浮空上升距离
    public int downedTime;//浮空完的倒地时间

    public int launchFirst; //命中未浮空敌人的浮空量
    public int launchNext;//命中已浮空敌人的浮空量
    public int launchMax;//浮空上限，当敌人浮空值大于此值无法命中
    public MoveEffect targetMoveEffect;//被防御时的移动效果

    [Header("伤害，破防，能量")]
    public float damage;
    public float defenseDecrease;
    public float superIncrease;

    
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.AirborneAttacked || target.currentState != CharacterState.Downed)//无法命中的状态
        {
            ApplyAirBorne(target, attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect, downedTime,damage,defenseDecrease,superIncrease);
        }
    }
}
