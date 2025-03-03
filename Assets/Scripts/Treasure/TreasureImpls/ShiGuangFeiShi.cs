using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiGuangFeiShi : TreasureBase
{
    public ShiGuangFeiShi(){
        Name = " ±π‚∑… ≈";
        Value = TreasureValue.Normal;
    }

    public void Effect(float Value){
        Value += 2.0f;
    }
}
