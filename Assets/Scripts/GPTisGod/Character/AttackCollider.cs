using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ʾ����������ײ����
public class AttackCollider : MonoBehaviour
{
    [HideInInspector]
    public GameObject Creator; // �ͷ���

    public Vector3 loc; // ������ͷ��ߵ�λ��

    public bool hit = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Creator)
        {
            hit = true;
            //Debug.Log("��ײ����⵽ " + collision.gameObject.name);
            // �������ʵ�ֶ�Ŀ����˺���Ч���߼�
        }
    }
}