using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string name; // 卡牌名称
    public int startupKe; // 出招所需的刻数
    public int activeKe; // 命中判定所需的刻数
    public int recoveryKe; // 收招所需的刻数
    public int damage; // 伤害值
    public CardEffect effect; // 卡牌附带的效果
    public float pushbackDistance; // 攻击防御敌人时的后退距离
    public AttackCollider attackColliderPrefab; // 攻击碰撞体预制体

    // 构造函数
    public Card(string name, int startupKe, int activeKe, int recoveryKe, int damage, CardEffect effect, float pushbackDistance, AttackCollider attackColliderPrefab)
    {
        this.name = name;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.damage = damage;
        this.effect = effect;
        this.pushbackDistance = pushbackDistance;
        this.attackColliderPrefab = attackColliderPrefab;
    }

    public void Execute(Character attacker, Character target)
    {
        // 设置出招阶段状态
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // 恢复时间流动以执行动作
        TimeManager.Instance.ResumeGame();

        // 延迟 startupKe 后进入命中判定阶段
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            attacker.SetState(CharacterState.AttackingActive, activeKe);

            // 创建攻击碰撞体
            if (attackColliderPrefab != null)
            {
                attacker.CreateCollider(attackColliderPrefab);
            }
        }, attacker));

        // 延迟 startupKe + activeKe 后进入收招阶段
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // 检测是否命中
            if (attacker.attackCollider.GetComponent<AttackCollider>().hit)
            {
                Debug.Log(target.gameObject);
                effect?.Trigger(target);
            }
        }, attacker));

        // 延迟 startupKe + activeKe + recoveryKe 后恢复为 Idle 状态并暂停时间
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            TimeManager.Instance.PauseGame(); // 玩家恢复为 Idle 状态后暂停游戏
        }, attacker));
    }
}
