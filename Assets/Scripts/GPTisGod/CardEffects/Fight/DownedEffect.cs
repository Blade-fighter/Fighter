using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownedEffect : CardEffect//倒地效果
{
    public int time;

    public override void Trigger(Character target, Character attacker)
    {
        target.SetState(CharacterState.Downed, time);//普通效果
    }
}
