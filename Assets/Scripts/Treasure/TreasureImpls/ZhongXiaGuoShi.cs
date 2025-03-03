using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZhongXiaGuoShi : TreasureBase
{
    public ZhongXiaGuoShi(){
        Name = "种下果实";
        Value = TreasureValue.Normal;
    }

    public void Effect(){
        TreasureManager.Instance.AddTreasure("", null);
    }
}
