using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeTrackerUI : MonoBehaviour
{
    public Text currentKeText; // ������ʾ��ǰ�������ı�
    public Text playerKeStatusText; // ������ʾ���״̬���ı�
    public Text enemyKeStatusText; // ������ʾ����״̬���ı�

    public Character playerCharacter;
    public Character enemyCharacter;

    void Start()
    {
        // ��ȡ��Һ͵��˵� Character ʵ��
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();
    }

    void Update()
    {
        // ʵʱ���µ�ǰ����
        currentKeText.text = "Current Ke: " + TimeManager.Instance.currentKe;

        // ������ҵ�״̬������Ϣ
        if (playerCharacter != null)
        {
            playerKeStatusText.text = "Player State: " + playerCharacter.currentState + "\nRemaining Ke: " + GetRemainingKe(playerCharacter);
        }

        // ���µ��˵�״̬������Ϣ
        if (enemyCharacter != null)
        {
            enemyKeStatusText.text = "Enemy State: " + enemyCharacter.currentState + "\nRemaining Ke: " + GetRemainingKe(enemyCharacter);
        }
    }

    private int GetRemainingKe(Character character)
    {
        // ��ȡ��ɫ��ǰ״̬ʣ��Ŀ���
        ScheduledAction action = ActionScheduler.Instance.GetScheduledActionForCharacter(character);
        if (action != null)
        {
            return action.executionKe - TimeManager.Instance.currentKe;
        }
        return 0;
    }
}