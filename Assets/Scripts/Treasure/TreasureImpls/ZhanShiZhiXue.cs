using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZhanShiZhiXue: TreasureBase
{
    public ZhanShiZhiXue(){
        Name = "ZhanShiZhiXue";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.BattleEnd);
    }

    public override void Effect(TreasureContext context){
        context.character.Heal(6);
    }
}
