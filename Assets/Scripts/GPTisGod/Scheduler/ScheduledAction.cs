public class ScheduledAction
{
    public int executionKe;
    public Character character; // ���ӶԽ�ɫ������
    private System.Action action;
    public bool isCancelled; // ���ڱ�Ǹö����Ƿ�ȡ��

    public ScheduledAction(int ke, System.Action act, Character character)
    {
        executionKe = ke;
        action = act;
        this.character = character;
        isCancelled = false; // ��ʼʱδ��ȡ��
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
        isCancelled = true; // ���Ϊ��ȡ��
    }
}
