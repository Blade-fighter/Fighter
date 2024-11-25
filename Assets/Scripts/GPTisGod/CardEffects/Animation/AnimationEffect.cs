using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation Effect", menuName = "Card Effects/Animation/Animation Effect")]
public class AnimationEffect : CardEffect
{
    [Header("打击动画对应的attackIndex值")]
    public int index;

    public override void Trigger(Character target, Character attacker)
    {
        attacker.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Attack");
        attacker.transform.GetChild(0).GetComponent<Animator>().SetInteger("AttackIndex",index);
    }
}
