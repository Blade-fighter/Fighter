using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureContext{
    public Character character;
    public Character enemy;
    public EffectTime effectTime;
    public int CardCount; // 连招中使用的牌数
    public int TotalCardCount; // 总共的卡牌数目
    public int CurrentBattleCount; // 当前战斗数目
};

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager Instance = null;

    Dictionary<EffectTime, SortedSet<string>> EffectNameMap = new Dictionary<EffectTime, SortedSet<string>>();
    Dictionary<string, TreasureBase> NameTreasureMap = new Dictionary<string, TreasureBase>();

    void Awake(){
        Instance = this;
    }

    public void ApplyTreasure(TreasureContext context, EffectTime effectTime){
        if(!EffectNameMap.ContainsKey(effectTime)) return;
        foreach(string name in EffectNameMap[effectTime]){
            NameTreasureMap[name].Effect(context);
        }
    }

    public void AddTreasure(TreasureContext context, TreasureBase treasureBase){
        foreach(EffectTime effectTime in treasureBase.EffectTimes){
            if(!EffectNameMap.ContainsKey(effectTime)){
                EffectNameMap.Add(effectTime, new SortedSet<string>());
            }
            if(!EffectNameMap[effectTime].Contains(treasureBase.Name)){
                EffectNameMap[effectTime].Add(treasureBase.Name);
            }
        }
        NameTreasureMap.Add(treasureBase.Name, treasureBase);
        context.effectTime = EffectTime.Add;
        ApplyTreasure(context , EffectTime.Add);
    }

    public void RemoveTreasure(TreasureContext context, string treasureName){
        context.effectTime = EffectTime.Remove;
        ApplyTreasure(context, EffectTime.Remove);
        NameTreasureMap.Remove(treasureName);
    }
}
