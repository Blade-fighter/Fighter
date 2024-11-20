using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int currentKe; // ��ǰ����
    public float keDuration = 1.0f/60; // ÿ�̵�ʱ�䳤��
    private float timer;
    public bool isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.fixedDeltaTime = 1.0f / 120.0f; // �� `FixedUpdate` ����Ϊÿ��ִ�� 60 ��
    }

    private void Start()
    {
        PauseGame(); // ����ʱ��ͣ��Ϸ
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            timer += Time.deltaTime;

            if (timer >= keDuration)
            {
                timer -= keDuration;
                currentKe++;
                ActionScheduler.Instance.ProcessKe(currentKe);
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
    }
}
