using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScheduledAction
{
    public int executionKe;
    public Character character; // 增加对角色的引用
    private System.Action action;

    public ScheduledAction(int ke, System.Action act, Character character)
    {
        executionKe = ke;
        action = act;
        this.character = character;
    }

    public void Execute()
    {
        action.Invoke();
    }
}
