using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BianBenJIaLi : TreasureBase
{
    public BianBenJIaLi(){
        Name = "�䱾����";
        Value = TreasureValue.Normal;
    }

    public void Effect(float Value){
        Value += 1.0f;
    }
}
