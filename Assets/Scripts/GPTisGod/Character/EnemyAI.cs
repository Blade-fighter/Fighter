using UnityEngine;

// 示例：敌人AI类
public class EnemyAI : MonoBehaviour
{
    public Character enemyCharacter;
    public Card attackCard;

    void Start()
    {
        ScheduleNextAttack();
    }

    void ScheduleNextAttack()
    {
        Character player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        int nextAttackKe = TimeManager.Instance.currentKe + Random.Range(10, 20); // 随机安排下一次攻击
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(nextAttackKe, ExecuteAttack,player));
    }

    void ExecuteAttack()
    {
        Character player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        if (player != null)
        {
            attackCard.Execute(enemyCharacter, player);
        }
        ScheduleNextAttack(); // 再次安排下一次攻击
    }
}