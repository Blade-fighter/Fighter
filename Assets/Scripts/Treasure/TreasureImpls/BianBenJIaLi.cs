using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BianBenJiaLi : TreasureBase
{
    public BianBenJiaLi(){
        Name = "BianBenJiaLi";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Card);
    }

    public override void Effect(TreasureContext context){
        if(context.CardCount == 4){
            // 角色的力量加一
            // context.player.
        }
    }
}
