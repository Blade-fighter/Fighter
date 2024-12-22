using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "CardData/Card")]
public class CardData : ScriptableObject
{
    public string cardName;      // 卡牌名称
    public CardType cardType;    // 卡牌类型
    public string cardDescription; // 卡牌描述
    public Sprite cardImage;     // 卡牌图像
    public int startupKe;        // 出招所需的刻数,同时也是防御持续刻数
    public int activeKe;         // 命中判定所需的刻数
    public int recoveryKe;       // 收招所需的刻数
    public AttackCollider collider;   // 创建的碰撞体
    public CardEffect[] startEffect;  // 打出时的效果
    public CardEffect[] hitEffect;    // 命中时的效果
    public List<HitData> multiHitData; // 多段攻击的数据，如果为空则不是多段攻击

    //Card.cs没有引用的数据,用于敌人判断卡牌优先级
    [Header("敌人用于判断卡牌优先级的数据")]
    public float distance;//攻击有效距离

    public float startNum = 0;//起始值
    public float distanceValue;//距离判定值
    public float playerHPValue;//玩家血量判定值
    public float enemyHPValue;//自身血量判定值

    public float DefenseValue;//玩家防御的加值
    public float StunValue;//玩家眩晕的加值
    public float attackValue;//玩家正在攻击的加值

    public int handIndex = -1; // index of hand

}
public enum CardType //卡牌类型
{
    Attack,
    Defense,
    Launch,//浮空
    MultiHit,
    Move
}
