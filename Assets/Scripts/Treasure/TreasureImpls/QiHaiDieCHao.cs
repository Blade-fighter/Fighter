using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QiHaiDieCHao : TreasureBase
{
    public QiHaiDieCHao(){
        Name = "Æøº£µþ³±";
        Value = TreasureValue.Normal;
    }
    public void Effect(float Value){
        Value += 1.0f;
    }
}
