// 示例：角色类
using UnityEngine;

public class Character : MonoBehaviour
{
    public string cName;
    public CharacterState currentState;
    public Collider2D hitCollider; // 角色的碰撞器
    public Collider2D attackCollider; // 角色的攻击碰撞器
    public bool canAct; // 是否可以行动
    public Deck deck; // 角色的卡组
    public bool dir; // 角色方向，true 表示向左，false 表示向右

    public void SetState(CharacterState newState, int durationKe)
    {
        currentState = newState;
        canAct = false;
        ActionScheduler.Instance.ScheduleAction(new ScheduledAction(TimeManager.Instance.currentKe + durationKe, () =>
        {
            if (currentState == newState)
            {
                currentState = CharacterState.Idle;
                canAct = true; // 状态恢复到Idle，允许行动
            }
        }, this));
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

    public void MoveBack(float distance)
    {
        // 处理角色后退的逻辑
        transform.position += Vector3.left * distance;
        Debug.Log(cName + " moved back by " + distance + " units.");
    }

    public bool IsAtEdge()
    {
        // 检查角色是否在场景的边缘
        float sceneLeftEdge = -10f; // 假设场景左边缘的x坐标为-10
        float sceneRightEdge = 10f; // 假设场景右边缘的x坐标为10
        return transform.position.x <= sceneLeftEdge || transform.position.x >= sceneRightEdge;
    }

    public void CreateCollider(AttackCollider colliderPrefab)
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
        }
        else // 朝右
        {
            GameObject co = Instantiate(colliderPrefab.gameObject, loc0, colliderPrefab.transform.rotation);
            co.GetComponent<AttackCollider>().Creator = gameObject;
            attackCollider = co.GetComponent<Collider2D>();
        }
    }

    // 执行卡牌动作
    public void ExecuteCard(CardData cardData, Character target)
    {
        // 创建卡牌
        Card card = new Card(cardData.cardName, cardData.startupKe, cardData.activeKe, cardData.recoveryKe, cardData.damage, cardData.effect, 0, cardData.collider);

        // 执行卡牌逻辑
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