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
    public Slider PlayerGuardBar;
    public Text PlayerGuard;
    public Slider EnemyGuardBar;
    public Text EnemyGuard;

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
        UpdateGuardBar();
        UpdateSuperBar();
    }
    private void Update()
    {
        UpdateHealthBar();
        UpdateGuardBar();
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

    private void UpdateGuardBar()
    {
        float playerGuardPercent = (float)Player.currentGuardValue / Player.maxGuardValue;
        float enemyGuardPercent = (float)Player.currentGuardValue / Player.maxGuardValue;

        if (PlayerGuardBar.value!=playerGuardPercent)
        {
            // �����Ʒ����߼�
            PlayerGuard.text = (int)Player.currentGuardValue + "/" + (int)Player.maxGuardValue;
            PlayerGuardBar.value = playerGuardPercent;
        }
        if (EnemyGuardBar.value != enemyGuardPercent)
        {
            // �����Ʒ����߼�
            EnemyGuard.text = (int)Enemy.currentGuardValue + "/" + (int)Enemy.maxGuardValue;
            EnemyGuardBar.value = enemyGuardPercent;
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
