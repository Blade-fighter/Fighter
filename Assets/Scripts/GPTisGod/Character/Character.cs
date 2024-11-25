// 示例：角色类
using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Character : MonoBehaviour
{
    public string cName;
    public CharacterState currentState;
    public Collider2D hitCollider; // 角色的碰撞器
    public Collider2D attackCollider; // 角色的攻击碰撞器
    public bool canAct; // 是否可以行动
    public Deck deck; // 角色的卡组
    public bool dir; // 角色方向，true 表示向左，false 表示向右

    public float leftBoundary = -19f;  // 场景最左边的x坐标
    public float rightBoundary = 19f;  // 场景最右边的x坐标

    public ScheduledAction currentAction;

    public int launchValue = 0; // 角色当前的浮空值
    public float groundHeight= 0;//地面高度

    public bool shouldGoToDowned=true;//是否倒地标志位
    public int currentAirbornePriority = 0;//多个浮空招式连续命中时，用于判断是否倒地的值

    public Animator animator;
    public CharacterAnimation cAnim;
    private void Start()
    {
        animator =  transform.GetChild(0).GetComponent<Animator>();
        cAnim = GetComponent<CharacterAnimation>();
    }
    private void Update()
    {
        //防止角色飞出地图,愚蠢做法
        if (gameObject.transform.position.x >= rightBoundary)
        {
            gameObject.transform.position = new Vector3(rightBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        if (gameObject.transform.position.x <= leftBoundary)
        {
            gameObject.transform.position = new Vector3(leftBoundary, gameObject.transform.position.y, gameObject.transform.position.z);
        }

    }
    private void FixedUpdate()
    {
        //判断方向,很愚蠢的做法，但能用
        if (gameObject.tag == "Player")
        {
            dir = GameObject.FindWithTag("Enemy").transform.position.x<=gameObject.transform.position.x;
        }
        if (gameObject.tag == "Enemy")
        {
            dir = GameObject.FindWithTag("Player").transform.position.x < gameObject.transform.position.x;
        }
        //换边时,变为Idle后图像翻转
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

        // 如果当前有相同的状态，则取消当前的动作，以新持续时间为准
        if (currentState == newState && currentAction != null)
        {
            currentAction.Cancel();
        }
        Debug.Log(gameObject.name +"从" +currentState+ "进入" + newState + "时间" + durationKe + "刻");
        currentState = newState;
        canAct = false;
        cAnim.BasicAnimationController(newState);//动画

        // 创建新的调度动作
        currentAction = new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState)
            {
                currentState = CharacterState.Idle;
                canAct = true;
                cAnim.BasicAnimationController(CharacterState.Idle);//动画
                if (newState == CharacterState.Airborne)
                {
                    launchValue = 0; // 重置浮空值
                }
            }
        }, this);

        ActionScheduler.Instance.ScheduleAction(currentAction);
    }
    public void TakeDamage(int damage)
    {
        Debug.Log(cName + " took " + damage + " damage.");
        // 处理生命值减少的逻辑
    }

    public void Heal(int amount)
    {
        Debug.Log(cName + " healed " + amount + " health.");
        // 处理生命值增加的逻辑
    }

    public void MoveBack(float distance,int durationKe)
    {
        // 处理角色后退的逻辑
        MoveOverTime(-distance, durationKe);
        Debug.Log(cName + "后退" + distance + "距离，时间" +durationKe +"刻");
    }

    // 检查角色是否在版边
    public bool IsAtBoundary()
    {
        float characterPositionX = transform.position.x;

        if (characterPositionX <= leftBoundary || characterPositionX >= rightBoundary)
        {
            return true;
        }
        return false;
    }

    public GameObject CreateCollider(AttackCollider colliderPrefab)//创建攻击碰撞体
    {
        Vector3 createrLoc = transform.position; // 释放者位置
        Vector3 loc = colliderPrefab.loc; // 碰撞体相对释放者位置
        Vector3 loc0 = new Vector3(createrLoc.x + loc.x, createrLoc.y + loc.y, createrLoc.z + loc.z); // 在右侧时位置
        Vector3 loc1 = new Vector3(createrLoc.x - loc.x, createrLoc.y + loc.y, createrLoc.z + loc.z); // 在左侧时位置

        if (dir) // 朝左，位置相反，镜像翻转
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc1, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            co.transform.Rotate(0, 180, 0);
            attackCollider = co.GetComponent<Collider2D>();
            return co;
        }
        else // 朝右
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc0, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            attackCollider = co.GetComponent<Collider2D>();
            return co;
        }
    }

    // 执行卡牌动作
    public void ExecuteCard(CardData cardData, Character target)
    {
        // 创建卡牌
        Card card = new Card(cardData.cardName, cardData.cardType,cardData.cardDescription,cardData.cardImage,cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.collider, cardData.startEffect,cardData.hitEffect,cardData.multiHitData);

        // 执行卡牌逻辑
        card.Execute(this, target);
    }

    // 基于刻数实现平滑移动
    public void MoveOverTime(float distance, int durationKe)
    {
        if (durationKe <= 0) return; // 确保持续时间为正数

        // 确定移动方向
        Vector3 direction = dir ? Vector3.left : Vector3.right;
        float distancePerKe = distance / durationKe; // 每刻移动的距离
        Vector3 movePerKe = direction * distancePerKe;

        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + durationKe;
        transform.position += movePerKe;
        // 手动移动每一刻
        for (int i = 1; i < durationKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // 在每个刻时移动一部分距离
                transform.position += movePerKe;

                // 在最后一刻确保位置精确到达目标点
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
        if (airborneTimeKe <= 0) return; // 确保持续时间为正数
        int startKe = TimeManager.Instance.currentKe;
        int halfAirborneTimeKe = airborneTimeKe / 2;
        int extraKe = airborneTimeKe % 2;
        int peakKe = startKe + halfAirborneTimeKe + extraKe;
        int endKe = startKe + airborneTimeKe;
        float distancePerKeUp = airborneHeight / (halfAirborneTimeKe + extraKe); // 上升阶段每刻的距离
        float distancePerKeDown = (airborneHeight+transform.position.y-groundHeight) / halfAirborneTimeKe; // 下降阶段每刻的距离

        shouldGoToDowned = true;
        // 提升浮空优先级，确保新的浮空技具有更高的优先级
        currentAirbornePriority++;
        // 使用当前的优先级来控制浮空效果(如果触发新的浮空，这个函数就会失效)
        int priority = currentAirbornePriority;

        transform.position += new Vector3(0, distancePerKeUp, 0);//先走一步
        // 上升阶段
        for (int i = 0; i < halfAirborneTimeKe + extraKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // 如果标志位被取消，则不执行浮空效果
                if (priority < currentAirbornePriority) return;

                transform.position += new Vector3(0, distancePerKeUp, 0);
            }, this));
        }

        transform.position -= new Vector3(0, distancePerKeDown, 0);//先走一步
        // 下降阶段
        for (int i = 0; i < halfAirborneTimeKe; i++)
        {
            int keToExecute = peakKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // 如果标志位被取消，则不执行下降效果
                if (priority < currentAirbornePriority) return;

                transform.position -= new Vector3(0, distancePerKeDown, 0);

                // 在倒数第二刻确保位置精确到达地面（迫不得已的选项）
                if (TimeManager.Instance.currentKe == endKe-1)
                {
                    Debug.Log("浮空归位");
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                    launchValue = 0;
                }
            }, this));
        }

        // 安排倒地效果
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(startKe + airborneTimeKe, () =>
        {
            // 只有在标志位为 true，且浮空层级匹配时，才进入倒地状态
            if (shouldGoToDowned && priority == currentAirbornePriority)
            {
                SetState(CharacterState.Downed, downedTime); // 倒地效果
                launchValue = 0; // 重置浮空值
                currentAirbornePriority = 0; // 重置浮空优先级
            }
        }, this));
    }


    // 施加AirborneAttacked状态时，敌人在硬直时间内返回地面
    public void ApplyAirborneAttackedEffect(int recoveryKe)
    {
        if (recoveryKe <= 0) return; // 确保持续时间为正数
        int startKe = TimeManager.Instance.currentKe;
        int endKe = startKe + recoveryKe;
        float distancePerKeDown = transform.position.y / recoveryKe; // 每刻下降的距离

        // 提升浮空优先级，确保新的浮空技具有更高的优先级
        currentAirbornePriority++;
        int priority = currentAirbornePriority;

        transform.position -= new Vector3(0, distancePerKeDown, 0);//先走一步

        // 下降阶段
        for (int i = 0; i < recoveryKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // 如果标志位被取消，则不执行下降效果
                if (priority < currentAirbornePriority) return;

                transform.position -= new Vector3(0, distancePerKeDown, 0);

                // 在倒数第二刻确保位置精确到达地面（迫不得已的选项）
                if (TimeManager.Instance.currentKe == endKe - 1)
                {
                    Debug.Log("浮空受击归位");
                    transform.position = new Vector3(transform.position.x, groundHeight, transform.position.z);
                }
            }, this));
        }
    }
    public void StepMove(Vector2 moveVector, int durationKe)
    {
        if (durationKe <= 0) return; // 确保持续时间为正数

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

        transform.position += movePerKe;//先走一步
        for (int i = 1; i < durationKe; i++)
        {
            int keToExecute = startKe + i;
            ActionScheduler.Instance.ScheduleAction(new ScheduledAction(keToExecute, () =>
            {
                // 在每个刻时移动一部分距离
                transform.position += movePerKe;
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
    Downed,
    Thrown,
    MovingBack
}