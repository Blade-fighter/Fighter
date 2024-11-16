using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;      // ��������
    public string cardType;      //��������
    public string cardDescription;//��������
    public Sprite cardImage;     // ����ͼ��
    public int startupKe;        // ��������Ŀ���
    public int activeKe;         // �����ж�����Ŀ���
    public int recoveryKe;       // ��������Ŀ���
    public int damage;           // �˺�ֵ
    public CardEffect effect;    // ���Ƹ�����Ч��
    public AttackCollider collider; //��������ײ��
}
