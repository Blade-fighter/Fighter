using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Visual Effect", menuName = "Card Effects/Animation/Visual Effect")]
public class VisualEffect : CardEffect
{
    public GameObject visualEffectPrefab; // Ԥ������Ч
    public Vector3 offset; // ��Ч����ڽ�ɫ��λ��ƫ��
    public float duration; // ����ʱ��

    public override void Trigger(Character target, Character attacker)
    {
        if (visualEffectPrefab != null && attacker != null)
        {
            // ������Чʵ��
            Vector3 effectPosition = attacker.transform.position + offset;
            GameObject effectInstance = Instantiate(visualEffectPrefab, effectPosition, Quaternion.identity);

            // ������Чʵ����ָ���ĳ���ʱ���
            Destroy(effectInstance, duration);
        }
    }
}