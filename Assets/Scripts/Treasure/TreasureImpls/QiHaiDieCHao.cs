using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiHaiDieCHao : TreasureBase
{
    public QiHaiDieCHao(){
        Name = "��������";
        Value = TreasureValue.Normal;
    }
    public void Effect(float Value){
        Value += 1.0f;
    }
}
