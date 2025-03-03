using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance = null;

    int coin = 0;

    void Start()
    {
        Instance = this;
    }

    public void AddCoin(int num){
        coin += num;
    }

    public void RemoveCoin(int num){
        coin -= num;
    }
}
