using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New MultiHits", menuName = "Card Effects/HitsData")]
[System.Serializable]

//ʵ����CardData�����þ��У����ô����ļ�
public class HitData
{
    public int startupKe; // ��������Ŀ���
    public int activeKe;  // �����ж�����Ŀ���
    public float damage; // �˺�ֵ
    public float defenseDecrease;//�Ʒ�ֵ
    public float superIncrease;//����ɱ������
    public GameObject attackColliderPrefab; // ÿ�ι�����Ӧ����ײ��
    public List<CardEffect> startEffects; //ÿ������Ч��
    public List<CardEffect> hitEffects; // ÿ�ι�����Ч��
}
