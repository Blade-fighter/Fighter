using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Visual Effect", menuName = "Card Effects/Animation/Visual Effect")]
public class AnimationEffect : CardEffect
{
    public AnimationClip clip;

    public override void Trigger(Character target, Character attacker)
    {
        attacker.transform.GetChild(0).GetComponent<Animation>().clip = clip;
        
    }
}
