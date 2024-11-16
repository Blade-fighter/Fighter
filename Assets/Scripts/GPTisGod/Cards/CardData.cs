using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;      // 卡牌名称
    public string cardType;      //卡牌类型
    public string cardDescription;//卡牌描述
    public Sprite cardImage;     // 卡牌图像
    public int startupKe;        // 出招所需的刻数
    public int activeKe;         // 命中判定所需的刻数
    public int recoveryKe;       // 收招所需的刻数
    public int damage;           // 伤害值
    public CardEffect effect;    // 卡牌附带的效果
    public AttackCollider collider; //创建的碰撞体
}
