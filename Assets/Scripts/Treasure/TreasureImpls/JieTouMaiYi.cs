using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JieTouMaiYi : TreasureBase
{
    public JieTouMaiYi(){
        Name = "��ͷ����";
        Value = TreasureValue.Normal;
    }

    public void Effect(){
        CoinManager.Instance.AddCoin(5);
    }
}
