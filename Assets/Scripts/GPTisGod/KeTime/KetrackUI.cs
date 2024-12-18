using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeTrackerUI : MonoBehaviour
{
    public Text currentKeText;
    public Text playerStateText;
    public Text enemyStateText;

    public GameObject playerKeBar; // 玩家刻度表的父物体
    public GameObject enemyKeBar;  // 敌人刻度表的父物体

    public Color idleColor = Color.white;   // 默认Idle颜色
    public Color stunnedColor = Color.yellow; // Stunned状态颜色
    public Color otherStateColor = Color.red; // 其他状态颜色

    private Character playerCharacter;
    private Character enemyCharacter;

    private List<Image> playerKeImages = new List<Image>();
    private List<Image> enemyKeImages = new List<Image>();

    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

        // 初始化刻度表的 Image 列表
        foreach (Transform child in playerKeBar.transform)
        {
            playerKeImages.Add(child.GetComponent<Image>());
        }

        foreach (Transform child in enemyKeBar.transform)
        {
            enemyKeImages.Add(child.GetComponent<Image>());
        }
    }

    private void Update()
    {
        //currentKeText.text = "当前刻数: " + TimeManager.Instance.currentKe;

        UpdateKeBar(playerCharacter, playerKeImages, playerStateText);
        UpdateKeBar(enemyCharacter, enemyKeImages, enemyStateText);
    }

    private void UpdateKeBar(Character character, List<Image> keImages, Text stateText)
    {
        // 获取当前状态和剩余时间
        CharacterState state = character.currentState;
        ScheduledAction action = character.currentAction;

        int remainingKe = action != null ? action.executionKe - TimeManager.Instance.currentKe : 0;
        stateText.text =ChangeStateToWord(state) + ": " + remainingKe+"刻";

        // 更新刻度表颜色
        for (int i = 0; i < keImages.Count; i++)
        {
            if (i < remainingKe && state != CharacterState.Idle)
            {
                // 根据状态改变颜色
                if (state == CharacterState.Stunned)
                    keImages[i].color = stunnedColor;
                else
                    keImages[i].color = otherStateColor;
            }
            else
            {
                // 重置为默认颜色
                keImages[i].color = idleColor;
            }
        }
    }
    string ChangeStateToWord(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Idle:
                return "空闲";
            case CharacterState.Stunned:
                return "受击";
            case CharacterState.AttackingStartup:
                return "出招";
            case CharacterState.AttackingActive:
                return "判定";
            case CharacterState.Recovery:
                return "收招";
            case CharacterState.BlockedStunned:
                return "受击";
            case CharacterState.Airborne:
                return "浮空";
            case CharacterState.AirborneAttacked:
                return "无法被命中";
            case CharacterState.Defending:
                return "防御";
            case CharacterState.MovingFront:
                return "移动";
            case CharacterState.MovingBack:
                return "后撤";
            case CharacterState.Downed:
                return "倒地";
            case CharacterState.Thrown:
                return "被投";
            case CharacterState.Jumping://实际没有写跳跃招式把自己变成此状态的逻辑，等着加
                return "跳跃";
            default:
                return "未知";
        }
    }
}
