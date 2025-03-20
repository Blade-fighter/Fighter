using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangJinTieGu : TreasureBase
{
    public GangJinTieGu(){
        Name = "GangJinTieGu";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Add);
    }

    public override void Effect(TreasureContext context){
        context.character.currentDefenseValue += 50;
        context.character.maxDefenseValue += 50;
    }
}
