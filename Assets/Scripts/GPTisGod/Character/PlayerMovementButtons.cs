using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementButtons : MonoBehaviour
{
    public Button leftMoveButton;  // ���ư�ť
    public Button rightMoveButton; // ���ư�ť
    public Character playerCharacter; // ��ҽ�ɫ
    public int moveDurationKe = 5;  // �ƶ�����Ŀ���
    public float moveDistance = 2.0f;  // �ƶ��ľ���

    private bool isMoving = false; // ��־λ�����ڷ�ֹ�ظ����
    private bool isTimeResumed = false; // ��־λ������׷��ʱ���Ƿ�������

    void Start()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
        // �󶨰�ť����¼�
        leftMoveButton.onClick.AddListener(() => ExecuteMove(-moveDistance));
        rightMoveButton.onClick.AddListener(() => ExecuteMove(moveDistance));
    }

    // ִ���ƶ�
    void ExecuteMove(float distance)
    {
        if (playerCharacter != null && !CardUI.isCardEffectActive && !isMoving)
        {
            isMoving = true;
            // ���ð�ť�Ϳ���ʹ��
            leftMoveButton.interactable = false;
            rightMoveButton.interactable = false;
            CardUI.isCardEffectActive = true;

            if (playerCharacter.dir) // ������
            {
                playerCharacter.MoveOverTime(-distance, moveDurationKe);
            }
            else if(!playerCharacter.dir)
            {
                playerCharacter.MoveOverTime(distance, moveDurationKe);
            }

            ResumeTime(); // ȷ��ʱ����������ʹ currentKe ��������
            CharacterAnimation cAnim = playerCharacter.cAnim;
            cAnim.ResetAllTrigger();
            //�����߼�
            if ((playerCharacter.dir&&distance<0)||(!playerCharacter.dir && distance > 0))
            {
                playerCharacter.SetState(CharacterState.MovingFront, moveDurationKe);
            }
            else
            {
                playerCharacter.SetState(CharacterState.MovingBack, moveDurationKe);
            }

            // ���ƶ���ɺ����ð�ť�Ϳ���
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + moveDurationKe, () =>
            {
                playerCharacter.animator.SetTrigger("Idle");//�ص�Idle
                leftMoveButton.interactable = true;
                rightMoveButton.interactable = true;
                CardUI.isCardEffectActive = false;
                PauseTime(); // �ƶ���ɺ���ͣ��Ϸ
                isMoving = false;
            }, playerCharacter));
        }
    }

    // ��װʱ��ָ��ķ���
    void ResumeTime()
    {
        if (!isTimeResumed)
        {
            TimeManager.Instance.ResumeGame();
            isTimeResumed = true;
        }
    }

    // ��װʱ����ͣ�ķ���
    void PauseTime()
    {
        if (isTimeResumed)
        {
            TimeManager.Instance.PauseGame();
            isTimeResumed = false;
        }
    }
}
