using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuoDongJinGu : TreasureBase
{
    public HuoDongJinGu(){
        Name = "����";
        Value = TreasureValue.Normal;
    }

    public void Effect(Character character){
        character.Heal(1);
    }
}
