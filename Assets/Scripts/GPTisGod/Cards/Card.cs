using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string name; // 卡牌名称
    public string cardType;      //卡牌类型
    public string cardDescription;//卡牌描述
    public Sprite cardImage;     // 卡牌图像
    public int startupKe; // 出招所需的刻数
    public int activeKe; // 命中判定所需的刻数
    public int recoveryKe; // 收招所需的刻数
    public int damage; // 伤害值
    public CardEffect[] startEffect; // 卡牌打出时附带的效果
    public CardEffect[] hitEffect; // 卡牌命中时附带的效果
    public AttackCollider attackColliderPrefab; // 攻击碰撞体预制体

    private GameObject activeCollider; // 用于引用当前创建的碰撞体

    // 构造函数
    public Card(string name, string cardType, string cardDescription, Sprite cardImage, int startupKe, int activeKe, int recoveryKe, CardEffect[] startEffect, CardEffect[] hitEffect, AttackCollider attackColliderPrefab)
    {
        this.name = name;
        this.cardType = cardType;
        this.cardDescription = cardDescription;
        this.cardImage = cardImage;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.startEffect = startEffect;
        this.hitEffect = hitEffect;
        this.attackColliderPrefab = attackColliderPrefab;
    }

    public void Execute(Character attacker, Character target)
    {
        // 设置出招阶段状态
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // 恢复时间流动以执行动作
        TimeManager.Instance.ResumeGame();
        //起始效果触发
        foreach (CardEffect effect in startEffect)
        {
            effect?.Trigger(target, attacker);
        }
        // 延迟 startupKe 后进入命中判定阶段
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            attacker.SetState(CharacterState.AttackingActive, activeKe);

            // 创建攻击碰撞体
            if (attackColliderPrefab != null)
            {
                activeCollider = attacker.CreateCollider(attackColliderPrefab);
            }

            // 每刻检测命中判定
            for (int i = 0; i < activeKe; i++)
            {
                ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + i, () =>
                {
                    if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
                    {
                        Debug.Log("招式打中了"+target.gameObject);
                        foreach (CardEffect effect in hitEffect)
                        {
                            effect?.Trigger(target, attacker);
                        }
                        // 销毁攻击碰撞体，避免多次触发
                        GameObject.Destroy(activeCollider);
                        activeCollider = null;
                    }
                }, attacker));
            }
        }, attacker));

        // 延迟 startupKe + activeKe 后进入收招阶段
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // 如果碰撞体未销毁，则在收招阶段销毁
            if (activeCollider != null)
            {
                GameObject.Destroy(activeCollider);
            }
        }, attacker));

        // 延迟 startupKe + activeKe + recoveryKe 后恢复为 Idle 状态并暂停时间
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            TimeManager.Instance.PauseGame(); // 玩家恢复为 Idle 状态后暂停游戏

            // 通知 CardUI 卡牌效果已完成
            CardUI.CardEffectComplete();
        }, attacker));
    }
}
