using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HeadBars : MonoBehaviour
{
    public Character Player;
    public Character Enemy;

    [Header("������")]
    public Slider PlayerHealthBar;
    public Text PlayerHealth;
    public Slider EnemyHealthBar;
    public Text EnemyHealth;

    [Header("�Ʒ���")]
    public Slider PlayerDefenseBar;
    public Text PlayerDefense;
    public Slider EnemyDefenseBar;
    public Text EnemyDefense;

    [Header("����ɱ��")]
    public Slider PlayerSuperBar;
    public Text PlayerSuperNum;
    public Text PlayerSuperText;
    public Slider EnemySuperBar;
    public Text EnemySuperNum;
    public Text EnemySuperText;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
        Enemy = GameObject.FindWithTag("Enemy").GetComponent<Character>();
        UpdateHealthBar();
        UpdateDefenseBar();
        UpdateSuperBar();
    }
    private void Update()
    {
        UpdateHealthBar();
        UpdateDefenseBar();
        UpdateSuperBar();
    }

    private void UpdateHealthBar()
    {
        float playerHealthPercent = (float)Player.currentHealth / Player.maxHealth;
        if (PlayerHealthBar.value!=playerHealthPercent)
        {
            // �������Ѫ���߼�
            PlayerHealth.text = (int)Player.currentHealth +"/"+ (int)Player.maxHealth;
            PlayerHealthBar.value = playerHealthPercent;
        }
        float enemyHealthPercent = (float)Enemy.currentHealth / Enemy.maxHealth;
        if (EnemyHealthBar.value != enemyHealthPercent)
        {
            // ���µ���Ѫ���߼�
            EnemyHealth.text = (int)Enemy.currentHealth + "/" + (int)Enemy.maxHealth;
            EnemyHealthBar.value = enemyHealthPercent;
        }
    }

    private void UpdateDefenseBar()
    {
        float playerGuardPercent = (float)Player.currentDefenseValue / Player.maxDefenseValue;
        float enemyGuardPercent = (float)Player.currentDefenseValue / Player.maxDefenseValue;

        if (PlayerDefenseBar.value!=playerGuardPercent)
        {
            // �����Ʒ����߼�
            PlayerDefense.text = (int)Player.currentDefenseValue + "/" + (int)Player.maxDefenseValue;
            PlayerDefenseBar.value = playerGuardPercent;
        }
        if (EnemyDefenseBar.value != enemyGuardPercent)
        {
            // �����Ʒ����߼�
            EnemyDefense.text = (int)Enemy.currentDefenseValue + "/" + (int)Enemy.maxDefenseValue;
            EnemyDefenseBar.value = enemyGuardPercent;
        }
    }

    private void UpdateSuperBar()//����ɱ
    {
        int playerSuperCount = Player.currentSuperCount;
        float playerSuperPercent = (float)Player.currentSuperValue / Player.maxSuperValue;
        int enemySuperCount = Enemy.currentSuperCount;
        float enemySuperPercent = (float)Enemy.currentSuperValue / Enemy.maxSuperValue;

        if (PlayerSuperBar.value!=playerSuperPercent||PlayerSuperNum.text!= Player.currentSuperCount.ToString())//���
        {
            PlayerSuperNum.text = Player.currentSuperCount.ToString();
            PlayerSuperText.text = (int)Player.currentSuperValue + "/" + (int)Player.maxSuperValue;
            PlayerSuperBar.value = playerSuperPercent;
        }

        if (EnemySuperBar.value != enemySuperPercent || EnemySuperNum.text != Enemy.currentSuperCount.ToString())//����
        {
            EnemySuperNum.text = Enemy.currentSuperCount.ToString();
            EnemySuperText.text = (int)Enemy.currentSuperValue+"/"+(int)Enemy.maxSuperValue;
            EnemySuperBar.value = enemySuperPercent;
        }
    }
}
