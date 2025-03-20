using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiHaiDieChao : TreasureBase
{
    public QiHaiDieChao(){
        Name = "QiHaiDieChao";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Attack);
        EffectTimes.Add(EffectTime.Attacked);
        EffectTimes.Add(EffectTime.Defened);
    }
    public override void Effect(TreasureContext context){
        context.character.IncreaseSuperValue(1.0f);
    }
}
