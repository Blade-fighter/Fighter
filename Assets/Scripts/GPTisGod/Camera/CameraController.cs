using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1; // ��һ����ɫ�� Transform
    public Transform player2; // �ڶ�����ɫ�� Transform
    public float minX; // ���������С X λ��
    public float maxX; // ���������� X λ��

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").transform;
        player2 = GameObject.FindGameObjectWithTag("Enemy").transform;
    }
    private void Update()
    {
        // ��������ɫ�е�� X λ��
        float midpointX = (player1.position.x + player2.position.x) / 2;

        // ����������� X λ�ã�ʹ�䲻������Сֵ�����ֵ
        float clampedX = Mathf.Clamp(midpointX, minX, maxX);

        // ���������λ�ã����� X �����ƶ�
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
