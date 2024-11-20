// ʾ������ɫ��
using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public string cName;
    public CharacterState currentState;
    public Collider2D hitCollider; // ��ɫ����ײ��
    public Collider2D attackCollider; // ��ɫ�Ĺ�����ײ��
    public bool canAct; // �Ƿ�����ж�
    public Deck deck; // ��ɫ�Ŀ���
    public bool dir; // ��ɫ����true ��ʾ����false ��ʾ����

    public float leftBoundary = -19f;  // ��������ߵ�x����
    public float rightBoundary = 19f;  // �������ұߵ�x����

    public ScheduledAction currentAction;
    public int launchValue = 0; // ��ɫ��ǰ�ĸ���ֵ
    public float groundHeight=0;//����߶�

    private void Update()
    {
        //��ֹ��ɫ�ɳ���ͼ
        if (gameObject.transform.position.x >= rightBoundary)
        {
            gameObject.transform.position = new Vector3(rightBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x <= leftBoundary)
        {
            gameObject.transform.position = new Vector3(leftBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }
    public void SetState(CharacterState newState, int durationKe)
    {
        // �����ǰ����ͬ��״̬����ȡ����ǰ�Ķ��������³���ʱ��Ϊ׼
        if (currentState == newState && currentAction != null)
        {
            currentAction.Cancel();
        }
        Debug.Log(gameObject.name +"��" +currentState+ "����" + newState + "ʱ��" + durationKe + "��");
        currentState = newState;
        canAct = false;

        // �����µĵ��ȶ���
        currentAction = new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState)
            {
                currentState = CharacterState.Idle;
                canAct = true;
                if (newState == CharacterState.Airborne)
                {
                    launchValue = 0; // ���ø���ֵ
                }
            }
        }, this);

        ActionScheduler.Instance.ScheduleAction(currentAction);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(cName + " took " + damage + " damage.");
        // ��������ֵ���ٵ��߼�
    }

    public void Heal(int amount)
    {
        Debug.Log(cName + " healed " + amount + " health.");
        // ��������ֵ���ӵ��߼�
    }

    public void MoveBack(float distance,int durationKe)
    {
        // �����ɫ���˵��߼�
        MoveOverTime(-distance, durationKe);
        Debug.Log(cName + "����" + distance + "���룬ʱ��" +durationKe +"��");
    }

    // ����ɫ�Ƿ��ڰ��
    public bool IsAtBoundary()
    {
        float characterPositionX = transform.position.x;

        if (characterPositionX <= leftBoundary || characterPositionX >= rightBoundary)
        {
            return true;
        }
        return false;
    }

    public GameObject CreateCollider(AttackCollider colliderPrefab)//����������ײ��
    {
        Vector3 createrLoc = transform.position; // �ͷ���λ��
        Vector3 loc = colliderPrefab.loc; // ��ײ������ͷ���λ��
        Vector3 loc0 = new Vector3(createrLoc.x + loc.x, createrLoc.y + loc.y, createrLoc.z + loc.z); // ���Ҳ�ʱλ��
        Vector3 loc1 = new Vector3(createrLoc.x - loc.x, createrLoc.y + loc.y, createrLoc.z + loc.z); // �����ʱλ��

        if (dir) // ����λ���෴������ת
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc1, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            co.transform.Rotate(0, 180, 0);
            attackCollider = co.GetComponent<Collider2D>();
            return co;
        }
        else // ����
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc0, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            attackCollider = co.GetComponent<Collider2D>();
            return co;
        }
    }

    // ִ�п��ƶ���
    public void ExecuteCard(CardData cardData, Character target)
    {
        // ��������
        Card card = new Card(cardData.cardName, cardData.cardType,cardData.cardDescription,cardData.cardImage,cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.startEffect,cardData.hitEffect,cardData.collider);

        // ִ�п����߼�
        card.Execute(this, target);
    }

    // ���ڿ���ʵ��ƽ���ƶ�
    public void MoveOverTime(float distance, int durationKe)
    {
        if (durationKe <= 0) return; // ȷ������ʱ��Ϊ����

        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + durationKe;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = dir ? startPosition - new Vector3(distance, 0, 0) : startPosition + new Vector3(distance, 0, 0);

        for (int i = 0; i < durationKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                float t = (float)(TimeManager.Instance.currentKe - startKe) / durationKe;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);

                // �����һ��ȷ��λ�þ�ȷ����Ŀ���
                if (TimeManager.Instance.currentKe == endKe)
                {
                    transform.position = targetPosition;
                }
            }, this));
        }
    }

    public void ApplyAirborneEffect(float airborneHeight, int airborneTimeKe)
    {
        int startKe = TimeManager.Instance.currentKe;
        int halfAirborneTimeKe = airborneTimeKe / 2;
        int extraKe = airborneTimeKe % 2;
        int peakKe = startKe + halfAirborneTimeKe + extraKe;
        int endKe = startKe + airborneTimeKe;
        float startY = transform.position.y;
        float peakY = startY + airborneHeight;

        // �����׶�
        for (int i = 0; i < halfAirborneTimeKe + extraKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                float t = (float)(TimeManager.Instance.currentKe - startKe) / (halfAirborneTimeKe + extraKe);
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(startY, peakY, t), transform.position.z);
            }, this));
        }

        // �½��׶�
        for (int i = 0; i < halfAirborneTimeKe; i++)
        {
            int keToExecute = peakKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                float t = (float)(TimeManager.Instance.currentKe - peakKe) / halfAirborneTimeKe;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(peakY, groundHeight, t), transform.position.z);

                // �����һ��ȷ��λ�þ�ȷ�������
                if (TimeManager.Instance.currentKe == endKe)
                {
                    Debug.Log("���չ�λ");
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                }
            }, this));
        }
    }

    // ʩ��AirborneAttacked״̬ʱ��������Ӳֱʱ���ڷ��ص���
    public void ApplyAirborneAttackedEffect(int recoveryKe)
    {
        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + recoveryKe;
        float startHeight = transform.position.y;
        float targetHeight = groundHeight;

        for (int i = 0; i < recoveryKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                float t = (float)(TimeManager.Instance.currentKe - startKe) / recoveryKe;
                transform.position = new Vector3(transform.position.x, Mathf.Lerp(startHeight, targetHeight, t), transform.position.z);

                // �����һ��ȷ��λ�þ�ȷ�������߶�
                if (TimeManager.Instance.currentKe == endKe-1)
                {
                    //�����ܻ���λ
                    transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
                }
            }, this));
        }
    }

}

public enum CharacterState
{
    Idle,
    AttackingStartup,
    AttackingActive,
    Recovery,
    Defending,
    BlockedStunned,
    Stunned,
    Jumping,
    Airborne,
    AirborneAttacked,
    Thrown,
    MovingBack
}