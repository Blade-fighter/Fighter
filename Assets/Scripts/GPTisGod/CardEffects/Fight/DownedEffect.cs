using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownedEffect : CardEffect//����Ч��
{
    public int time;

    public override void Trigger(Character target, Character attacker)
    {
        target.SetState(CharacterState.Downed, time);//��ͨЧ��
    }
}
