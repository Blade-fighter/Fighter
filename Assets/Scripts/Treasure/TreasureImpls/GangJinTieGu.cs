using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GangJinTieGu : TreasureBase
{
    public GangJinTieGu(){
        Name = "�ֽ�����";
        Value = TreasureValue.Normal;
    }

    public void Effect(Character character){
        character.currentDefenseValue += 50;
        character.maxDefenseValue += 50;
    }
}
