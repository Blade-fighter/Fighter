using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player1; // 第一个角色的 Transform
    public Transform player2; // 第二个角色的 Transform
    public float minX; // 摄像机的最小 X 位置
    public float maxX; // 摄像机的最大 X 位置

    private void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player").transform;
        player2 = GameObject.FindGameObjectWithTag("Enemy").transform;
    }
    private void Update()
    {
        // 计算两角色中点的 X 位置
        float midpointX = (player1.position.x + player2.position.x) / 2;

        // 限制摄像机的 X 位置，使其不超过最小值和最大值
        float clampedX = Mathf.Clamp(midpointX, minX, maxX);

        // 更新摄像机位置，仅在 X 轴上移动
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}
