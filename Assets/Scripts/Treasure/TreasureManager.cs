using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager Instance = null;
    Dictionary<string, TreasureBase> Treasures;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void AddTreasure(string TreasureName, TreasureBase treasureBase){

        if(TreasureName == "Æ¤²ÚÈâºñ"){
            PiCaoRouHou treasure = (PiCaoRouHou)treasureBase;
            Character character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            treasure.Effect(character);
        }else if(TreasureName == "¸Ö½îÌú¹Ç"){
            GangJinTieGu treasure = (GangJinTieGu)treasureBase;
            Character character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            treasure.Effect(character);
        }
        Treasures.Add(TreasureName, treasureBase);
    }

    public void RemoveTreasure(string TreasureName){
        if(TreasureName == "Æ¤²ÚÈâºñ"){
            PiCaoRouHou treasure = (PiCaoRouHou)GetTreasure("Æ¤²ÚÈâºñ");
            Character character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            treasure.DeEffect(character);
        }
        Treasures.Remove(TreasureName);
    }

    public bool IsValid(string TreasureName){
        return Treasures.ContainsKey(TreasureName);
    }

    public TreasureBase GetTreasure(string TreasureName){
        TreasureBase result;
        Treasures.TryGetValue(TreasureName, out result);
        return result;
    }
}
