using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaiQuZiRu : TreasureBase 
{
    public LaiQuZiRu(){
        Name = "LaiQuZiRu";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.BattleBegin);
    }

    public override void Effect(TreasureContext context){

    }
}
