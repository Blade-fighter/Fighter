using System.Collections.Generic;
using UnityEngine;
public class ActionScheduler : MonoBehaviour
{
    public static ActionScheduler Instance;

    private List<ScheduledAction> actionQueue = new List<ScheduledAction>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ScheduleAction(ScheduledAction action)
    {
        actionQueue.Add(action);
    }

    public void ProcessKe(int currentKe)
    {
        // 对 actionQueue 按 executionKe 进行排序，确保判定动作优先执行
        actionQueue.Sort((a, b) => a.executionKe.CompareTo(b.executionKe));

        for (int i = actionQueue.Count - 1; i >= 0; i--)
        {
            if (actionQueue[i].executionKe <= currentKe)
            {
                if (!actionQueue[i].isCancelled)
                {
                    actionQueue[i].Execute();
                }
                actionQueue.RemoveAt(i);
            }
        }
    }

    public ScheduledAction GetScheduledActionForCharacter(Character character)
    {
        foreach (ScheduledAction action in actionQueue)
        {
            if (action.character == character)
            {
                return action;
            }
        }
        return null;
    }
}


