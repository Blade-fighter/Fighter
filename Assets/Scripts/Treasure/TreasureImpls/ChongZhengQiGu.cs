using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChongZhengQiGu : TreasureBase
{
    public ChongZhengQiGu(){
        Name = "ChongZhengQiGu";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Shuffle);
    }

    public override void Effect(TreasureContext context){
        context.character.Heal(context.TotalCardCount / 5.0f);
    }
}
