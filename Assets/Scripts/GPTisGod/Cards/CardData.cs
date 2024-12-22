using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Card", menuName = "CardData/Card")]
public class CardData : ScriptableObject
{
    public string cardName;      // ��������
    public CardType cardType;    // ��������
    public string cardDescription; // ��������
    public Sprite cardImage;     // ����ͼ��
    public int startupKe;        // ��������Ŀ���,ͬʱҲ�Ƿ�����������
    public int activeKe;         // �����ж�����Ŀ���
    public int recoveryKe;       // ��������Ŀ���
    public AttackCollider collider;   // ��������ײ��
    public CardEffect[] startEffect;  // ���ʱ��Ч��
    public CardEffect[] hitEffect;    // ����ʱ��Ч��
    public List<HitData> multiHitData; // ��ι��������ݣ����Ϊ�����Ƕ�ι���

    //Card.csû�����õ�����,���ڵ����жϿ������ȼ�
    [Header("���������жϿ������ȼ�������")]
    public float distance;//������Ч����

    public float startNum = 0;//��ʼֵ
    public float distanceValue;//�����ж�ֵ
    public float playerHPValue;//���Ѫ���ж�ֵ
    public float enemyHPValue;//����Ѫ���ж�ֵ

    public float DefenseValue;//��ҷ����ļ�ֵ
    public float StunValue;//���ѣ�εļ�ֵ
    public float attackValue;//������ڹ����ļ�ֵ

    public int handIndex = -1; // index of hand

}
public enum CardType //��������
{
    Attack,
    Defense,
    Launch,//����
    MultiHit,
    Move
}
