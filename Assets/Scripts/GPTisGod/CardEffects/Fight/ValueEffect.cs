using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Value Effect", menuName = "Card Effects/Fight/Value Effect")]
public class ValueEffect : CardEffect
{
    //������ֱ��д��Card����
    //�������ֵЧ�������ʵ��,�����������Լ���Ѫ������
    [Header("Ŀ��Ĭ��Ϊ���ˣ���ѡĿ��Ϊ�Լ�")]
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