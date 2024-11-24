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


    public MoveEffect targetMoveEffect;
    
    public override void Trigger(Character target, Character attacker)
    {
        ApplyAirBorne(target, attacker, defendingHardness, airborneTime, airborneValue, launchFirst, launchNext, launchMax, targetMoveEffect,downedTime);
    }
}
