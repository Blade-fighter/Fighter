using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JieTouMaiYi : TreasureBase
{
    public JieTouMaiYi(){
        Name = "JieTouMaiYi";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Ablility);
    }

    public override void Effect(TreasureContext context){
        CoinManager.Instance.AddCoin(5);
    }
}
