using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeTrackerUI : MonoBehaviour
{
    public Text currentKeText;
    public Text playerStateText;
    public Text enemyStateText;

    public GameObject playerKeBar; // ��ҿ̶ȱ�ĸ�����
    public GameObject enemyKeBar;  // ���˿̶ȱ�ĸ�����

    public Color idleColor = Color.white;   // Ĭ��Idle��ɫ
    public Color stunnedColor = Color.yellow; // Stunned״̬��ɫ
    public Color otherStateColor = Color.red; // ����״̬��ɫ

    private Character playerCharacter;
    private Character enemyCharacter;

    private List<Image> playerKeImages = new List<Image>();
    private List<Image> enemyKeImages = new List<Image>();

    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemyCharacter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>();

        // ��ʼ���̶ȱ�� Image �б�
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
        //currentKeText.text = "��ǰ����: " + TimeManager.Instance.currentKe;

        UpdateKeBar(playerCharacter, playerKeImages, playerStateText);
        UpdateKeBar(enemyCharacter, enemyKeImages, enemyStateText);
    }

    private void UpdateKeBar(Character character, List<Image> keImages, Text stateText)
    {
        // ��ȡ��ǰ״̬��ʣ��ʱ��
        CharacterState state = character.currentState;
        ScheduledAction action = character.currentAction;

        int remainingKe = action != null ? action.executionKe - TimeManager.Instance.currentKe : 0;
        if (state==CharacterState.Idle)//idle����ʾʱ��
        {
            remainingKe = 0;
        }
        stateText.text =ChangeStateToWord(state) + ": " + remainingKe+"��";

        // ���¿̶ȱ���ɫ
        for (int i = 0; i < keImages.Count; i++)
        {
            if (i < remainingKe && state != CharacterState.Idle)
            {
                // ����״̬�ı���ɫ
                keImages[i].color = ChooseColor(state);
            }
            else
            {
                // ����ΪĬ����ɫ
                keImages[i].color = idleColor;
            }
        }
    }
    Color ChooseColor(CharacterState state)
    {
        switch(state)
        {
            case CharacterState.Idle:
                return idleColor;
            case CharacterState.Stunned:
                return Color.yellow;
            case CharacterState.AttackingStartup:
                return Color.green;
            case CharacterState.AttackingActive:
                return Color.red;
            case CharacterState.Recovery:
                return Color.blue;
            case CharacterState.BlockedStunned:
                return Color.yellow;
            case CharacterState.Airborne:
                return Color.yellow;
            case CharacterState.AirborneAttacked:
                return Color.yellow;
            case CharacterState.Defending:
                return Color.yellow;
            case CharacterState.MovingFront:
                return Color.yellow;
            case CharacterState.MovingBack:
                return Color.yellow;
            case CharacterState.Downed:
                return Color.yellow;
            case CharacterState.Thrown:
                return Color.yellow;
            case CharacterState.Jumping://ʵ��û��д��Ծ��ʽ���Լ���ɴ�״̬���߼������ż�
                return Color.yellow;
            default:
                return Color.white;
        }
    }

    string ChangeStateToWord(CharacterState state)
    {
        switch (state)
        {
            case CharacterState.Idle:
                return "����";
            case CharacterState.Stunned:
                return "�ܻ�";
            case CharacterState.AttackingStartup:
                return "����";
            case CharacterState.AttackingActive:
                return "�ж�";
            case CharacterState.Recovery:
                return "����";
            case CharacterState.BlockedStunned:
                return "�ܻ�";
            case CharacterState.Airborne:
                return "����";
            case CharacterState.AirborneAttacked:
                return "�޷�������";
            case CharacterState.Defending:
                return "����";
            case CharacterState.MovingFront:
                return "�ƶ�";
            case CharacterState.MovingBack:
                return "��";
            case CharacterState.Downed:
                return "����";
            case CharacterState.Thrown:
                return "��Ͷ";
            case CharacterState.Jumping://ʵ��û��д��Ծ��ʽ���Լ���ɴ�״̬���߼������ż�
                return "��Ծ";
            default:
                return "δ֪";
        }
    }
}
