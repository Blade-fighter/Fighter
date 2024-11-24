using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Visual Effect", menuName = "Card Effects/Animation/Visual Effect")]
public class VisualEffect : CardEffect
{
    public GameObject visualEffectPrefab; // 预制体特效
    public Vector3 offset; // 特效相对于角色的位置偏移
    public float duration; // 持续时间

    public override void Trigger(Character target, Character attacker)
    {
        if (visualEffectPrefab != null && attacker != null)
        {
            // 创建特效实例
            Vector3 effectPosition = attacker.transform.position + offset;
            GameObject effectInstance = Instantiate(visualEffectPrefab, effectPosition, Quaternion.identity);

            // 销毁特效实例在指定的持续时间后
            Destroy(effectInstance, duration);
        }
    }
}