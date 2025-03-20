using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiGuangFeiShi : TreasureBase
{
    public ShiGuangFeiShi(){
        Name = "ShiGuangFeiShi";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.BattleEnd);
    }

    public override void Effect(TreasureContext context){
        context.character.maxHealth += 2.0f;
    }
}
