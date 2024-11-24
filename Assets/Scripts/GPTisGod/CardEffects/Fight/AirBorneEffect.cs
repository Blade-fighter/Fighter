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


    public MoveEffect targetMoveEffect;
    
    public override void Trigger(Character target, Character attacker)
    {
        ApplyAirBorne(target, attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect,downedTime);
    }
}
