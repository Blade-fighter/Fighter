using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreasureValue{
    Normal,
    Rare,
    Boss
};

public class TreasureBase : MonoBehaviour
{
    public string Name;
    public TreasureValue Value;
}
