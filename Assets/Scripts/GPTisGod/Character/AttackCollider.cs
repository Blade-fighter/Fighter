using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 示例：攻击碰撞体类
public class AttackCollider : MonoBehaviour
{
    [HideInInspector]
    public GameObject Creator; // 释放者

    public Vector3 loc; // 相对于释放者的位置

    public bool hit = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Creator)
        {
            hit = true;
            //Debug.Log("碰撞器检测到 " + collision.gameObject.name);
            // 这里可以实现对目标的伤害或效果逻辑
        }
    }
}