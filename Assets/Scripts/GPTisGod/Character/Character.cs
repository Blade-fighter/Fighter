// ʾ������ɫ��
using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;
using System;
using UnityEditor.Experimental.GraphView;

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
    public float groundHeight= 0;//����߶�

    public bool shouldGoToDowned=true;//�Ƿ񵹵ر�־λ
    public int currentAirbornePriority = 0;//���������ʽ��������ʱ�������ж��Ƿ񵹵ص�ֵ

    public Animator animator;
    public CharacterAnimation cAnim;

    public float maxHealth = 100;
    public float currentHealth = 100;

    public float currentDefenseValue = 50;
    public float maxDefenseValue = 50;

    public int currentSuperCount = 0;
    public int maxSuperCount = 3;
    public float currentSuperValue = 0;
    public float maxSuperValue = 50;
    private void Start()
    {
        animator =  transform.GetChild(0).GetComponent<Animator>();
        cAnim = GetComponent<CharacterAnimation>();
    }
    private void Update()
    {
        //��ֹ��ɫ�ɳ���ͼ,�޴�����
        if (gameObject.transform.position.x >= rightBoundary)
        {
            gameObject.transform.position = new Vector3(rightBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x <= leftBoundary)
        {
            gameObject.transform.position = new Vector3(leftBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        //�����ҽ�ɫ��ΪIdle���ָ�ʱ�����������������ȡ��֮��Ļ��ƣ�����ø�
        if (gameObject.tag == "Player" && currentState != CharacterState.Idle)
        {
            TimeManager.Instance.ResumeGame();
        }
        if (gameObject.tag == "Player" && currentState == CharacterState.Idle)
        {
            TimeManager.Instance.PauseGame();
        }
    }
    private void FixedUpdate()
    {
        //�жϷ���,���޴���������������
        if (gameObject.tag == "Player")
        {
            dir = GameObject.FindWithTag("Enemy").transform.position.x<=gameObject.transform.position.x;
        }
        if (gameObject.tag == "Enemy")
        {
            dir = GameObject.FindWithTag("Player").transform.position.x < gameObject.transform.position.x;
        }
        //����ʱ,��ΪIdle��ͼ��ת
        if (dir && currentState == CharacterState.Idle)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(!dir && currentState==CharacterState.Idle)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    public void SetState(CharacterState newState, int durationKe)
    {

        // �����ǰ����ͬ��״̬����ȡ����ǰ�Ķ��������³���ʱ��Ϊ׼
        if (currentState == newState && currentAction != null)
        {
            currentAction.Cancel();
        }
        //Debug.Log(gameObject.name +"��" +currentState+ "����" + newState + "ʱ��" + durationKe + "��");
        currentState = newState;
        canAct = false;
        cAnim.BasicAnimationController(newState);//����

        // �����µĵ��ȶ���
        currentAction = new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState&&currentState!=CharacterState.AttackingActive && currentState != CharacterState.AttackingStartup)
            {
                if (currentState == CharacterState.Stunned || currentState == CharacterState.Downed || currentState == CharacterState.BlockedStunned || currentState == CharacterState.Thrown)
                    cAnim.BasicAnimationController(CharacterState.Idle);//��Ϊ��ť�Ϳ��ƴ�����Ч��������ָ�Idle
                if(currentState!=CharacterState.Airborne)//�ų�������Ӱ�쵹��Ч��
                    currentState = CharacterState.Idle;
                canAct = true;
                if (newState == CharacterState.Airborne)
                    launchValue = 0; // ���ø���ֵ
            }
        }, this);

        ActionScheduler.Instance.ScheduleAction(currentAction);
    }

    public void MoveBack(float distance,int durationKe)
    {
        // �����ɫ���˵��߼�
        MoveOverTime(-distance, durationKe);
        //Debug.Log(cName + "����" + distance + "���룬ʱ��" +durationKe +"��");
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
        Card card = new Card(cardData.cardName, cardData.cardType,cardData.cardDescription,cardData.cardImage,cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.collider, cardData.startEffect,cardData.hitEffect,cardData.multiHitData);

        // ִ�п����߼�
        card.Execute(this, target);
    }

    // ���ڿ���ʵ��ƽ���ƶ�
    public void MoveOverTime(float distance, int durationKe)
    {
        if (durationKe <= 0) return; // ȷ������ʱ��Ϊ����

        // ȷ���ƶ�����
        Vector3 direction = dir ? Vector3.left : Vector3.right;
        float distancePerKe = distance / durationKe; // ÿ���ƶ��ľ���
        Vector3 movePerKe = direction * distancePerKe;

        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + durationKe;
        transform.position += movePerKe;
        // �ֶ��ƶ�ÿһ��
        for (int i = 1; i < durationKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // ��ÿ����ʱ�ƶ�һ���־���
                transform.position += movePerKe;

                // �����һ��ȷ��λ�þ�ȷ����Ŀ���
                if (TimeManager.Instance.currentKe == endKe)
                {
                    Vector3 targetPosition = transform.position + direction * (distancePerKe * (durationKe - i - 1));
                    transform.position = targetPosition;
                }
            }, this));
        }
    }
    public void ApplyAirborneEffect(float airborneHeight, int airborneTimeKe, int downedTime)
    {
        if (airborneTimeKe <= 0) return; // ȷ������ʱ��Ϊ����
        int startKe = TimeManager.Instance.currentKe;
        int halfAirborneTimeKe = airborneTimeKe / 2;
        int extraKe = airborneTimeKe % 2;
        int peakKe = startKe + halfAirborneTimeKe + extraKe;
        int endKe = startKe + airborneTimeKe;
        float distancePerKeUp = airborneHeight / (halfAirborneTimeKe + extraKe); // �����׶�ÿ�̵ľ���
        float distancePerKeDown = (airborneHeight+transform.position.y-groundHeight) / halfAirborneTimeKe; // �½��׶�ÿ�̵ľ���

        shouldGoToDowned = true;
        // �����������ȼ���ȷ���µĸ��ռ����и��ߵ����ȼ�
        currentAirbornePriority++;
        // ʹ�õ�ǰ�����ȼ������Ƹ���Ч��(��������µĸ��գ���������ͻ�ʧЧ)
        int priority = currentAirbornePriority;

        transform.position += new Vector3(0, distancePerKeUp, 0);//����һ��
        // �����׶�
        for (int i = 0; i < halfAirborneTimeKe + extraKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // �����־λ��ȡ������ִ�и���Ч��
                if (priority < currentAirbornePriority) return;

                transform.position += new Vector3(0, distancePerKeUp, 0);
            }, this));
        }

        transform.position -= new Vector3(0, distancePerKeDown, 0);//����һ��
        // �½��׶�
        for (int i = 0; i < halfAirborneTimeKe; i++)
        {
            int keToExecute = peakKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // �����־λ��ȡ������ִ���½�Ч��
                if (priority < currentAirbornePriority) return;

                transform.position -= new Vector3(0, distancePerKeDown, 0);

                // �ڵ����ڶ���ȷ��λ�þ�ȷ������棨�Ȳ����ѵ�ѡ�
                if (TimeManager.Instance.currentKe == endKe-1)
                {
                    //Debug.Log("���չ�λ");
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                    launchValue = 0;
                }
            }, this));
        }

        // ���ŵ���Ч��
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(startKe + airborneTimeKe, () =>
        {
            // ֻ���ڱ�־λΪ true���Ҹ��ղ㼶ƥ��ʱ���Ž��뵹��״̬
            if (shouldGoToDowned && priority == currentAirbornePriority)
            {
                SetState(CharacterState.Downed, downedTime); // ����Ч��
                launchValue = 0; // ���ø���ֵ
                currentAirbornePriority = 0; // ���ø������ȼ�
            }
        }, this));
    }


    // ʩ��AirborneAttacked״̬ʱ��������Ӳֱʱ���ڷ��ص���
    public void ApplyAirborneAttackedEffect(int recoveryKe)
    {
        if (recoveryKe <= 0) return; // ȷ������ʱ��Ϊ����
        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + recoveryKe;
        float distancePerKeDown = transform.position.y / recoveryKe; // ÿ���½��ľ���

        // �����������ȼ���ȷ���µĸ��ռ����и��ߵ����ȼ�
        currentAirbornePriority++;
        int priority = currentAirbornePriority;

        transform.position -= new Vector3(0, distancePerKeDown, 0);//����һ��

        // �½��׶�
        for (int i = 0; i < recoveryKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // �����־λ��ȡ������ִ���½�Ч��
                if (priority < currentAirbornePriority) return;

                transform.position -= new Vector3(0, distancePerKeDown, 0);

                // �ڵ����ڶ���ȷ��λ�þ�ȷ������棨�Ȳ����ѵ�ѡ�
                if (TimeManager.Instance.currentKe == endKe - 1)
                {
                    Debug.Log("�����ܻ���λ");
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                }
            }, this));
        }
    }
    public void StepMove(Vector2 moveVector, int durationKe)
    {
        if (durationKe <= 0) return; // ȷ������ʱ��Ϊ����

        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + durationKe;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = Vector3.zero;
        if (dir)
        {
            targetPosition = startPosition + new Vector3(-moveVector.x, moveVector.y, 0);
        }
        else
        {
            targetPosition = startPosition + new Vector3(moveVector.x, moveVector.y, 0);
        }
        Vector3 movePerKe = new Vector3(moveVector.x / durationKe, moveVector.y / durationKe, 0);

        transform.position += movePerKe;//����һ��
        for (int i = 1; i < durationKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // ��ÿ����ʱ�ƶ�һ���־���
                transform.position += movePerKe;
            }, this));
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        if(currentHealth>maxHealth)
            currentHealth = maxHealth;
    }

    // �Ʒ����߼� - ����ֵ
    public void DecreaseDefenseValue(float value)
    {
        currentDefenseValue -= value;
        if (currentDefenseValue <= 0)
        {
            currentDefenseValue = 50;
            SetState(CharacterState.Stunned,20); // ����ѣ��״̬
        }
    }

    // ����ɱ���߼� - ���ӳ���ɱ��ֵ
    public void IncreaseSuperValue(float value)
    {
        if(gameObject.tag == "Player"){
            /*
            if(TreasureManager.Instance.IsValid("QiHaiDieChao")){
                QiHaiDieCHao treasure = (QiHaiDieCHao)TreasureManager.Instance.GetTreasure("QiHaiDieChao");
                treasure.Effect(value);
            }
            */
        }
        currentSuperValue += value;
        if (currentSuperValue >= 50&&currentSuperCount<maxSuperCount)
        {
            currentSuperCount++;
            if (currentSuperCount < 3)
            {
                currentSuperValue -= 50;
            }
            else
            {
                currentSuperValue = 50;
            }
        }
    }

    // ���ĳ���ɱ��
    public void UseSuperMeter(int count)
    {
        if(TreasureManager.Instance.IsValid("��ͷ����")){
            JieTouMaiYi treasure = (JieTouMaiYi)TreasureManager.Instance.GetTreasure("��ͷ����");
            treasure.Effect();
        }
        if (currentSuperCount >= count)
        {
            currentSuperCount -= count;
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
    Downed,
    Thrown,
    MovingFront,
    MovingBack
}