using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZhanShiZhiXue: TreasureBase
{
    public ZhanShiZhiXue(){
        Name = "սʿ֮Ѫ";
        Value = TreasureValue.Normal;
    }
    void Start(){
        
    }
    public void Effect(Character character){
        character.Heal(6);
    }
}
