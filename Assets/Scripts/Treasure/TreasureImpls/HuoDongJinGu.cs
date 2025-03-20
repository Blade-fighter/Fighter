using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuoDongJinGu : TreasureBase
{
    public HuoDongJinGu(){
        Name = "HuoDongJinGu";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Card);
    }

    public override void Effect(TreasureContext context){
        if(context.CardCount % 4 == 0){
            context.character.Heal(1);
        }
    }
}
