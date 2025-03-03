using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PiCaoRouHou : TreasureBase
{
    public PiCaoRouHou(){
        Name = "Æ¤²ÚÈâºñ";
        Value = TreasureValue.Normal;
    }

    public void Effect(Character character){
        character.maxHealth += 10.0f;
        character.currentHealth += 10.0f;
    }

    public void DeEffect(Character character){
        character.maxHealth -= 10.0f;
        character.currentHealth = Math.Min(character.maxHealth, character.currentHealth);
    }
}
