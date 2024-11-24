using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Fight/Heal Effect")]
public class HealEffect : CardEffect
{
    public int healAmount; // жнафа©

    public override void Trigger(Character target, Character attacker)
    {
        target.Heal(healAmount);
        Debug.Log(target.name + " is healed for " + healAmount + " health.");
    }
}
