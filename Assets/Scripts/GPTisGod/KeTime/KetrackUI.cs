using UnityEngine;
using UnityEngine.UI;

public class KeTrackerUI : MonoBehaviour
{
    public Text currentKeText;
    public Text playerKeText;
    public Text enemyKeText;
    public Character playerCharacter;
    public Character enemyCharacter;

    private void Start()
    {
        // 获取玩家和敌人的 Character 实例
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }
    void Update()
    {
        currentKeText.text = "当前刻数: "+TimeManager.Instance.currentKe;
        // 获取玩家和敌人的当前调度动作，并获取其剩余的刻数
        ScheduledAction playerAction = playerCharacter.currentAction;
        ScheduledAction enemyAction = enemyCharacter.currentAction;

        // 如果玩家有调度动作，显示其剩余的刻数
        if (playerAction != null&&playerCharacter.currentState!=CharacterState.Idle)
        {
            playerKeText.text = "玩家状态: " + playerCharacter.currentState + " 剩余刻数: " + (playerAction.executionKe - TimeManager.Instance.currentKe);
        }
        else
        {
            playerKeText.text = "玩家状态: " + playerCharacter.currentState + " 剩余刻数: 0";
        }

        // 如果敌人有调度动作，显示其剩余的刻数
        if (enemyAction != null && enemyCharacter.currentState != CharacterState.Idle)
        {
            enemyKeText.text = "敌人状态: " + enemyCharacter.currentState + " 剩余刻数: " + (enemyAction.executionKe - TimeManager.Instance.currentKe);
        }
        else
        {
            enemyKeText.text = "敌人状态: " + enemyCharacter.currentState + " 剩余刻数: 0";
        }
    }
}
