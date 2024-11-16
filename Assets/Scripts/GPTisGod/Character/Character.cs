// ʾ������ɫ��
using UnityEngine;

public class Character : MonoBehaviour
{
    public string cName;
    public CharacterState currentState;
    public Collider2D hitCollider; // ��ɫ����ײ��
    public Collider2D attackCollider; // ��ɫ�Ĺ�����ײ��
    public bool canAct; // �Ƿ�����ж�
    public Deck deck; // ��ɫ�Ŀ���
    public bool dir; // ��ɫ����true ��ʾ����false ��ʾ����

    public void SetState(CharacterState newState, int durationKe)
    {
        currentState = newState;
        canAct = false;
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState)
            {
                currentState = CharacterState.Idle;
                canAct = true; // ״̬�ָ���Idle�������ж�
            }
        }, this));
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

    public void MoveBack(float distance)
    {
        // �����ɫ���˵��߼�
        transform.position += Vector3.left * distance;
        Debug.Log(cName + " moved back by " + distance + " units.");
    }

    public bool IsAtEdge()
    {
        // ����ɫ�Ƿ��ڳ����ı�Ե
        float sceneLeftEdge = -10f; // ���賡�����Ե��x����Ϊ-10
        float sceneRightEdge = 10f; // ���賡���ұ�Ե��x����Ϊ10
        return transform.position.x <= sceneLeftEdge || transform.position.x >= sceneRightEdge;
    }

    public void CreateCollider(AttackCollider colliderPrefab)
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
        }
        else // ����
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc0, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            attackCollider = co.GetComponent<Collider2D>();
        }
    }

    // ִ�п��ƶ���
    public void ExecuteCard(CardData cardData, Character target)
    {
        // ��������
        Card card = new Card(cardData.cardName, cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.damage, cardData.effect, 0, cardData.collider);

        // ִ�п����߼�
        card.Execute(this, target);
    }
}

public enum CharacterState
{
    Idle,
    AttackingStartup,
    AttackingActive,
    Recovery,
    Defending,
    Stunned,
    Airborne,
    Thrown,
    MovingBack
}