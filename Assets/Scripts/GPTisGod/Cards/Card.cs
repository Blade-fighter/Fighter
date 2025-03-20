using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Card
{
    public string name; // 卡牌名称
    public CardType cardType;      //卡牌类型
    public string cardDescription;//卡牌描述
    public Sprite cardImage;     // 卡牌图像
    public float damage; // 伤害值
    public float defenseDecrease;//破防值
    public float superIncrease;//超必杀增加量
    public int startupKe; // 出招所需的刻数
    public int activeKe; // 命中判定所需的刻数
    public int recoveryKe; // 收招所需的刻数,多段招式只需设置这个
    public CardEffect[] startEffect; // 卡牌打出时附带的效果
    public CardEffect[] hitEffect; // 卡牌命中时附带的效果
    public AttackCollider attackColliderPrefab; // 攻击碰撞体预制体

    public GameObject activeCollider; // 用于引用当前创建的碰撞体
    static int executeTimes = 0;

    public List<HitData> multiHitData; // 多段攻击的数据
    // 构造函数
    public Card(string name, CardType cardType, string cardDescription, Sprite cardImage,int startupKe, int activeKe, int recoveryKe, AttackCollider attackColliderPrefab, CardEffect[] startEffect, CardEffect[] hitEffect,List<HitData> multiHitData)
    {
        this.name = name;
        this.cardType = cardType;
        this.cardDescription = cardDescription;
        this.cardImage = cardImage;
        this.startupKe = startupKe;
        this.activeKe = activeKe;
        this.recoveryKe = recoveryKe;
        this.attackColliderPrefab = attackColliderPrefab;
        this.startEffect = startEffect;
        this.hitEffect = hitEffect;
        this.multiHitData = multiHitData;
    }

    public void Execute(Character attacker, Character target)
    {
        if(attacker.gameObject.tag == "Player"){
            executeTimes += 1;
            TreasureContext context = new TreasureContext();
            context.CardCount = executeTimes;
            context.character = attacker;
            TreasureManager.Instance.ApplyTreasure(context, EffectTime.Card);
        }

        switch (cardType)
        {
            case CardType.Defense://防御
                ExecuteDefense(attacker);
                break;
            case CardType.Attack://普通打击技
                ExecuteAttack(attacker, target);
                break;
            case CardType.Launch://浮空技
                ExecuteAttack(attacker, target);
                break;
            case CardType.MultiHit:
                ExecuteMultiHitAttack(attacker, target);
                break;
            case CardType.Move:
                ExecuteMove(attacker, target);
                break;
            // 其他类型的卡牌可以在这里继续添加
            default:
                Debug.LogWarning("未知的卡牌类型: " + cardType);
                break;
        }
    }

    private void ExecuteDefense(Character character)//防御的代码
    {
        // 设置角色为防御状态
        character.SetState(CharacterState.Defending, startupKe);

        // 恢复时间流动，以执行动作
        if(character.tag == "Player")
        {
            TimeManager.Instance.ResumeGame(); 
        }

        // 延迟 startupKe 后恢复为 Idle 状态并暂停时间
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe, () =>
        {
            character.SetState(CharacterState.Idle, 0);
            if (character.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }
            character.animator.SetInteger("AttackIndex", 0);//重置招式index
            // 通知 CardUI 卡牌效果已完成
            CardUI.CardEffectComplete();
        }, character));
    }


    public void ExecuteAttack(Character attacker, Character target)
    {
        attacker.SetState(CharacterState.AttackingStartup, startupKe);

        // 恢复时间流动以执行动作
        if (attacker.tag == "Player")
        {
            TimeManager.Instance.ResumeGame();
        }
        // 起始效果触发
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

        }, attacker));

        // 在每一刻的命中判定中进行检测
        for (int i = 0; i < activeKe; i++)
        {
            int keToExecute = TimeManager.Instance.currentKe + startupKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
                {
                    //Debug.Log("招式打中了: " + target.gameObject.name);
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

        // 延迟 startupKe + activeKe 后进入收招阶段
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe, () =>
        {
            attacker.SetState(CharacterState.Recovery, recoveryKe);

            // 如果碰撞体未销毁，则在收招阶段销毁
            if (activeCollider != null)
            {   
                //收招时再判定一次
                if (activeCollider.GetComponent<AttackCollider>().hit)
                {
                    //Debug.Log("招式打中了: " + target.gameObject.name);
                    foreach (CardEffect effect in hitEffect)
                    {
                        effect?.Trigger(target, attacker);
                    }
                    // 销毁攻击碰撞体，避免多次触发
                    GameObject.Destroy(activeCollider);
                    activeCollider = null;
                }
                //Debug.Log(TimeManager.Instance.currentKe + "刻时候" + activeCollider + "销毁了");
                GameObject.Destroy(activeCollider);
            }
        }, attacker));

        // 延迟 startupKe + activeKe + recoveryKe 后恢复为 Idle 状态并暂停时间
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }
            attacker.animator.SetInteger("AttackIndex", 0);//重置招式index
            // 通知 CardUI 卡牌效果已完成
            CardUI.CardEffectComplete();
        }, attacker));
    }
    private void ExecuteMultiHitAttack(Character attacker, Character target)
    {
        int currentHitIndex = 0;

        void ExecuteNextHit()
        {
            if (currentHitIndex >= multiHitData.Count)
            {
                // 所有攻击段已完成，进入收招阶段
                attacker.SetState(CharacterState.Recovery, recoveryKe);
                // 延迟 startupKe + activeKe + recoveryKe 后恢复为 Idle 状态并暂停时间
                ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
                {
                    attacker.SetState(CharacterState.Idle, 0);
                    if (attacker.tag == "Player")
                    {
                        TimeManager.Instance.PauseGame();
                    }
                    attacker.animator.SetInteger("AttackIndex", 0);//重置招式index
                    // 通知 CardUI 卡牌效果已完成
                    CardUI.CardEffectComplete();
                }, attacker));
                return;
            }

            HitData currentHit = multiHitData[currentHitIndex];
            attacker.SetState(CharacterState.AttackingStartup, currentHit.startupKe);
            // 恢复时间流动以执行动作
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.ResumeGame();
            }

            foreach (CardEffect effect in currentHit.startEffects)//每段的起始效果
            {
                effect?.Trigger(target, attacker);
            }
            // 创建碰撞体
            GameObject activeCollider = attacker.CreateCollider(currentHit.attackColliderPrefab.GetComponent<AttackCollider>());

            // 延迟 startupKe 后进入命中判定阶段
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + currentHit.startupKe, () =>
            {
                attacker.SetState(CharacterState.AttackingActive, currentHit.activeKe);

                // 命中判定逻辑
                ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + currentHit.activeKe, () =>
                {
                    if (activeCollider != null && activeCollider.GetComponent<AttackCollider>().hit)
                    {
                        //Debug.Log("第" + (currentHitIndex + 1) + "段攻击命中: " + target.gameObject.name);
                        foreach (CardEffect effect in currentHit.hitEffects)
                        {
                            effect?.Trigger(target, attacker);
                        }
                    }

                    // 销毁当前碰撞体
                    GameObject.Destroy(activeCollider);

                    // 调度下一段攻击
                    currentHitIndex++;
                    ExecuteNextHit();
                }, attacker));
            }, attacker));
        }

        // 执行第一段攻击
        ExecuteNextHit();
    }

    //纯移动代码，测试用
    public void ExecuteMove(Character attacker, Character target)
    {
        // 恢复时间流动以执行动作
        if (attacker.tag == "Player")
        {
            TimeManager.Instance.ResumeGame();
        }

        // 起始效果触发
        foreach (CardEffect effect in startEffect)
        {
            effect?.Trigger(target, attacker);
        }
        // 延迟 startupKe + activeKe + recoveryKe 后恢复为 Idle 状态并暂停时间
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + startupKe + activeKe + recoveryKe, () =>
        {
            attacker.SetState(CharacterState.Idle, 0);
            if (attacker.tag == "Player")
            {
                TimeManager.Instance.PauseGame();
            }

            // 通知 CardUI 卡牌效果已完成
            CardUI.CardEffectComplete();
        }, attacker));
    }
}


