using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangJinTieGu : TreasureBase
{
    public GangJinTieGu(){
        Name = "¸Ö½îÌú¹Ç";
        Value = TreasureValue.Normal;
    }

    public void Effect(Character character){
        character.currentDefenseValue += 50;
        character.maxDefenseValue += 50;
    }
}
