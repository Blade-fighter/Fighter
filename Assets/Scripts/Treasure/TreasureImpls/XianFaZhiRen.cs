using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XianFaZhiRen : TreasureBase 
{
    public XianFaZhiRen(){
        Name = "XianFaZhiRen";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Attack);
    }

    public override void Effect(TreasureContext context){
        if(context.character.currentHealth == context.character.maxHealth){
            // 击中敌人的招式翻倍
            // context.character.
        }
    }
}
