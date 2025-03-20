using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TreasureValue{
    Normal,
    Rare,
    Boss
};

public enum EffectTime{
    Add,
    Remove,
    BattleEnd,
    Attack, // 命中
    Attacked, // 受击
    Defened, // 格挡
    BattleBegin,
    Card, // 出牌
    Shuffle, // 洗牌
    Ablility, // 技能
    Shop, // 处于商店时

    Max,
};

public class TreasureBase : MonoBehaviour
{
    public string Name;
    public TreasureValue Value;
    public List<EffectTime> EffectTimes;

    public virtual void Effect(TreasureContext context){

    }
}
