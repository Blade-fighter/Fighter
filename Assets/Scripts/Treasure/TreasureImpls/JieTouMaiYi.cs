using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JieTouMaiYi : TreasureBase
{
    public JieTouMaiYi(){
        Name = "Ω÷Õ∑¬Ù“’";
        Value = TreasureValue.Normal;
    }

    public void Effect(){
        CoinManager.Instance.AddCoin(5);
    }
}
