// 示例：玩家控制器类
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Character playerCharacter;
    public Character enemyCharacter; // 当前敌人


    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

    }
    void Update()
    {
        if (playerCharacter.canAct)
        {
            TimeManager.Instance.PauseGame(); // 暂停游戏，等待玩家输入

            if (Input.GetMouseButtonDown(0)) // 监听鼠标左键按下以开始拖拽卡牌
            {
                // 这里应该实现卡牌的拖拽逻辑，例如选择手牌中的卡牌并显示其拖拽效果
                Debug.Log("Card is being dragged");
            }
            else if (Input.GetMouseButtonUp(0)) // 鼠标左键松开以释放卡牌
            {
                // 这里应该实现卡牌释放的逻辑，例如确定卡牌的目标并执行卡牌效果
                Debug.Log("Card released");
            }

            TimeManager.Instance.ResumeGame(); // 玩家操作完成后恢复游戏
        }
    }
}