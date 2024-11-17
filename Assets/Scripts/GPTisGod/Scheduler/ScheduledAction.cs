public class ScheduledAction
{
    public int executionKe;
    public Character character; // 增加对角色的引用
    private System.Action action;
    public bool isCancelled; // 用于标记该动作是否被取消

    public ScheduledAction(int ke, System.Action act, Character character)
    {
        executionKe = ke;
        action = act;
        this.character = character;
        isCancelled = false; // 初始时未被取消
    }

    public void Execute()
    {
        if (!isCancelled)
        {
            action.Invoke();
        }
    }

    public void Cancel()
    {
        isCancelled = true; // 标记为已取消
    }
}
