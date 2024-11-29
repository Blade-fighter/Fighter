using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Effect", menuName = "Card Effects/Fight/Damage Effect")]
public class DamageEffect : CardEffect
{
    public int damageAmount; // жнафа©

    public override void Trigger(Character target, Character attacker)
    {
        target.TakeDamage(damageAmount);
    }
}
