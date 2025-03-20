using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiCaoRouHou : TreasureBase
{
    public PiCaoRouHou(){
        Name = "PiCaoRouHou";
        Value = TreasureValue.Normal;
        EffectTimes.Add(EffectTime.Add);
        EffectTimes.Add(EffectTime.Remove);
    }

    public override void Effect(TreasureContext context){
        if(context.effectTime == EffectTime.Add){
            context.character.maxHealth += 10.0f;
            context.character.currentHealth += 10.0f;
        }else{
            context.character.maxHealth -= 10.0f;
            context.character.currentHealth = Math.Min(context.character.maxHealth, context.character.currentHealth);
        }
    }
}
