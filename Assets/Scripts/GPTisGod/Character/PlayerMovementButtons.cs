using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementButtons : MonoBehaviour
{
    public Button leftMoveButton;  // 左移按钮
    public Button rightMoveButton; // 右移按钮
    public Character playerCharacter; // 玩家角色
    public int moveDurationKe = 5;  // 移动所需的刻数
    public float moveDistance = 2.0f;  // 移动的距离

    private bool isMoving = false; // 标志位，用于防止重复点击
    private bool isTimeResumed = false; // 标志位，用于追踪时间是否在流动

    void Start()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
        // 绑定按钮点击事件
        leftMoveButton.onClick.AddListener(() => ExecuteMove(-moveDistance));
        rightMoveButton.onClick.AddListener(() => ExecuteMove(moveDistance));
    }

    // 执行移动
    void ExecuteMove(float distance)
    {
        if (playerCharacter != null && !CardUI.isCardEffectActive && !isMoving)
        {
            isMoving = true;
            // 禁用按钮和卡牌使用
            leftMoveButton.interactable = false;
            rightMoveButton.interactable = false;
            CardUI.isCardEffectActive = true;

            if (playerCharacter.dir) // 朝左反向
            {
                playerCharacter.MoveOverTime(-distance, moveDurationKe);
            }
            else if(!playerCharacter.dir)
            {
                playerCharacter.MoveOverTime(distance, moveDurationKe);
            }

            ResumeTime(); // 确保时间流动，以使 currentKe 继续更新
            CharacterAnimation cAnim = playerCharacter.cAnim;
            cAnim.ResetAllTrigger();
            //动画逻辑
            if ((playerCharacter.dir&&distance<0)||(!playerCharacter.dir && distance > 0))
            {
                playerCharacter.SetState(CharacterState.MovingFront, moveDurationKe);
            }
            else
            {
                playerCharacter.SetState(CharacterState.MovingBack, moveDurationKe);
            }

            // 在移动完成后启用按钮和卡牌
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + moveDurationKe, () =>
            {
                playerCharacter.animator.SetTrigger("Idle");//回到Idle
                leftMoveButton.interactable = true;
                rightMoveButton.interactable = true;
                CardUI.isCardEffectActive = false;
                PauseTime(); // 移动完成后暂停游戏
                isMoving = false;
            }, playerCharacter));
        }
    }

    // 封装时间恢复的方法
    void ResumeTime()
    {
        if (!isTimeResumed)
        {
            TimeManager.Instance.ResumeGame();
            isTimeResumed = true;
        }
    }

    // 封装时间暂停的方法
    void PauseTime()
    {
        if (isTimeResumed)
        {
            TimeManager.Instance.PauseGame();
            isTimeResumed = false;
        }
    }
}
