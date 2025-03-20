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
    Attack, // ����
    Attacked, // �ܻ�
    Defened, // ��
    BattleBegin,
    Card, // ����
    Shuffle, // ϴ��
    Ablility, // ����
    Shop, // �����̵�ʱ

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
