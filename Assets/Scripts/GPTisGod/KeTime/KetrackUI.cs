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
        // ��ȡ��Һ͵��˵� Character ʵ��
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }
    void Update()
    {
        currentKeText.text = "��ǰ����: "+TimeManager.Instance.currentKe;
        // ��ȡ��Һ͵��˵ĵ�ǰ���ȶ���������ȡ��ʣ��Ŀ���
        ScheduledAction playerAction = playerCharacter.currentAction;
        ScheduledAction enemyAction = enemyCharacter.currentAction;

        // �������е��ȶ�������ʾ��ʣ��Ŀ���
        if (playerAction != null&&playerCharacter.currentState!=CharacterState.Idle)
        {
            playerKeText.text = "���״̬: " + playerCharacter.currentState + " ʣ�����: " + (playerAction.executionKe - TimeManager.Instance.currentKe);
        }
        else
        {
            playerKeText.text = "���״̬: " + playerCharacter.currentState + " ʣ�����: 0";
        }

        // ��������е��ȶ�������ʾ��ʣ��Ŀ���
        if (enemyAction != null && enemyCharacter.currentState != CharacterState.Idle)
        {
            enemyKeText.text = "����״̬: " + enemyCharacter.currentState + " ʣ�����: " + (enemyAction.executionKe - TimeManager.Instance.currentKe);
        }
        else
        {
            enemyKeText.text = "����״̬: " + enemyCharacter.currentState + " ʣ�����: 0";
        }
    }
}
