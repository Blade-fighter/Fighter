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
            // ���е��˵���ʽ����
            // context.character.
        }
    }
}
