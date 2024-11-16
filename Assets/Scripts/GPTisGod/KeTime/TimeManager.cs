using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public int currentKe; // ��ǰ����
    public float keDuration = 0.05f; // ÿ�̵�ʱ�䳤��
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
        PauseGame(); // ����ʱ��ͣ��Ϸ
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
