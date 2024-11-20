using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int currentKe; // 当前刻数
    public float keDuration = 1.0f/60; // 每刻的时间长度
    private float timer;
    public bool isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        Time.fixedDeltaTime = 1.0f / 120.0f; // 将 `FixedUpdate` 调整为每秒执行 60 次
    }

    private void Start()
    {
        PauseGame(); // 启动时暂停游戏
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
