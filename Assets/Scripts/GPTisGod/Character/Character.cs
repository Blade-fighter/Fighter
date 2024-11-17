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

    private ScheduledAction currentAction;

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

        currentState = newState;
        canAct = false;

        // �����µĵ��ȶ���
        currentAction = new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState)
            {
                currentState = CharacterState.Idle;
                canAct = true;
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
    // ʹ��Э����ʵ��ƽ���ƶ�
    public void MoveOverTime(float distance, int durationKe)
    {
        if (durationKe <= 0) return; // ȷ������ʱ��Ϊ����

        float duration = durationKe * TimeManager.Instance.keDuration; // �ܳ���ʱ��
        Vector3 startPosition = transform.position;
        if (dir)//���󣬷���
        {
            Vector3 targetPosition = startPosition - new Vector3(distance, 0, 0);
            StartCoroutine(SmoothMove(startPosition, targetPosition, duration));
        }
        else//���ң�����
        {
            Vector3 targetPosition = startPosition + new Vector3(distance, 0, 0);
            StartCoroutine(SmoothMove(startPosition, targetPosition, duration));
        }
    }

    // Э�̣�������һ��ʱ����ƽ���ƶ�
    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // �ȴ���һ֡
        }

        transform.position = targetPosition; // ȷ����������λ��
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
    Thrown,
    MovingBack
}