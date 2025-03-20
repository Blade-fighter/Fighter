using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhongXiaGuoShi : TreasureBase
{
    public ZhongXiaGuoShi(){
        Name = "ZhongXiaGuoShi";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.BattleEnd);
    }

    public override void Effect(TreasureContext context){
        if(context.CurrentBattleCount % 5 == 0){
            // TreasureManager.Instance.AddTreasure("", null);
        }
    }
}
