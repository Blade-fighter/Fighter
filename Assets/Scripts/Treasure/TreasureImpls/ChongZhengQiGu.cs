using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChongZhengQiGu : TreasureBase
{
    public ChongZhengQiGu(){
        Name = "ÖØÕûÆì¹Ä";
        Value = TreasureValue.Normal;
    }

    public void Effect(float CardCount, Character character){
        character.Heal(CardCount / 5.0f);
    }
}
