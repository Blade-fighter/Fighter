// 示例：角色类
using UnityEngine;
using System.Collections;

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

    private ScheduledAction currentAction;

    private void Update()
    {
        //防止角色飞出地图
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
        // 如果当前有相同的状态，则取消当前的动作，以新持续时间为准
        if (currentState == newState && currentAction != null)
        {
            currentAction.Cancel();
        }

        currentState = newState;
        canAct = false;

        // 创建新的调度动作
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
        Card card = new Card(cardData.cardName, cardData.cardType,cardData.cardDescription,cardData.cardImage,cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.startEffect,cardData.hitEffect,cardData.collider);

        // 执行卡牌逻辑
        card.Execute(this, target);
    }
    // 使用协程来实现平滑移动
    public void MoveOverTime(float distance, int durationKe)
    {
        if (durationKe <= 0) return; // 确保持续时间为正数

        float duration = durationKe * TimeManager.Instance.keDuration; // 总持续时间
        Vector3 startPosition = transform.position;
        if (dir)//朝左，反向
        {
            Vector3 targetPosition = startPosition - new Vector3(distance, 0, 0);
            StartCoroutine(SmoothMove(startPosition, targetPosition, duration));
        }
        else//朝右，正向
        {
            Vector3 targetPosition = startPosition + new Vector3(distance, 0, 0);
            StartCoroutine(SmoothMove(startPosition, targetPosition, duration));
        }
    }

    // 协程，用于在一定时间内平滑移动
    private IEnumerator SmoothMove(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null; // 等待下一帧
        }

        transform.position = targetPosition; // 确保到达最终位置
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