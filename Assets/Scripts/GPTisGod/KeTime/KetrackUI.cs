using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeTrackerUI : MonoBehaviour
{
    public Text currentKeText; // 用于显示当前刻数的文本
    public Text playerKeStatusText; // 用于显示玩家状态的文本
    public Text enemyKeStatusText; // 用于显示敌人状态的文本

    public Character playerCharacter;
    public Character enemyCharacter;

    void Start()
    {
        // 获取玩家和敌人的 Character 实例
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    void Update()
    {
        // 实时更新当前刻数
        currentKeText.text = "Current Ke: " + TimeManager.Instance.currentKe;

        // 更新玩家的状态刻数信息
        if (playerCharacter != null)
        {
            playerKeStatusText.text = "Player State: " + playerCharacter.currentState + "\nRemaining Ke: " + GetRemainingKe(playerCharacter);
        }

        // 更新敌人的状态刻数信息
        if (enemyCharacter != null)
        {
            enemyKeStatusText.text = "Enemy State: " + enemyCharacter.currentState + "\nRemaining Ke: " + GetRemainingKe(enemyCharacter);
        }
    }

    private int GetRemainingKe(Character character)
    {
        // 获取角色当前状态剩余的刻数
        ScheduledAction action = ActionScheduler.Instance.GetScheduledActionForCharacter(character);
        if (action != null)
        {
            return action.executionKe - TimeManager.Instance.currentKe;
        }
        return 0;
    }
}