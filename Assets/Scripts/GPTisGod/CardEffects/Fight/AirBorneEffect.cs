using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AirBorne Effect", menuName = "Card Effects/Fight/AirBorne Effect")]
public class AirBorneEffect : CardEffect
{
    public int defendingHardness; // ����ʱ��Ӳֱʱ��

    public int airborneTime;//���ӵĸ���ʱ��
    public float airborneValue;//������������
    public int downedTime;//������ĵ���ʱ��

    public int launchFirst; //����δ���յ��˵ĸ�����
    public int launchNext;//�����Ѹ��յ��˵ĸ�����
    public int launchMax;//�������ޣ������˸���ֵ���ڴ�ֵ�޷�����
    public MoveEffect targetMoveEffect;//������ʱ���ƶ�Ч��

    [Header("�˺����Ʒ�������")]
    public float damage;
    public float defenseDecrease;
    public float superIncrease;

    
    public override void Trigger(Character target, Character attacker)
    {
        if (target.currentState != CharacterState.AirborneAttacked || target.currentState != CharacterState.Downed)//�޷����е�״̬
        {
            ApplyAirBorne(target, attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect, downedTime,damage,defenseDecrease,superIncrease);
        }
    }
}
