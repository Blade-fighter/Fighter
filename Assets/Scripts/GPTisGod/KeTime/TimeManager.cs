using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int currentKe; // 当前刻数
    public float keDuration = 0.05f; // 每刻的时间长度
    private float timer;
    private bool isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        PauseGame(); // 启动时暂停游戏
    }

    void Update()
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
