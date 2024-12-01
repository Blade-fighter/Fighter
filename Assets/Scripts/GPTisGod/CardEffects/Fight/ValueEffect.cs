using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Value Effect", menuName = "Card Effects/Fight/Value Effect")]
public class ValueEffect : CardEffect
{
    //基础的直接写在Card里了
    //额外的数值效果用这个实现,比如打出反而自己扣血的招数
    [Header("目标默认为敌人，勾选目标为自己")]
    bool toSelf;
    public float heal;
    public float damage;
    public float superIncrease;
    public float defenseDecrease;

    public override void Trigger(Character target, Character attacker)
    {
        Character character = toSelf ? attacker : target;
        if (heal != 0)
            character.Heal(heal);
        if(superIncrease!=0)
            character.IncreaseSuperValue(superIncrease);
        if (damage != 0)
            character.TakeDamage(damage);
        if (defenseDecrease != 0)
            character.DecreaseDefenseValue(defenseDecrease);
    }
}