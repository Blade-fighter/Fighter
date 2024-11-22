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
    public AnimationClip clip;//��Ӧ�Ķ���
    public CardEffect[] startEffect;  // ���ʱ��Ч��
    public CardEffect[] hitEffect;    // ����ʱ��Ч��
    public List<HitData> multiHitData; // ��ι��������ݣ����Ϊ�����Ƕ�ι���
}
public enum CardType //��������
{
    Attack,
    Defense,
    Launch,
    MultiHit,
    Move
}
