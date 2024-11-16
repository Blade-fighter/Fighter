using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target);

    // 应用硬直效果
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {
        if (target.currentState == CharacterState.Defending)
        {
            target.SetState(CharacterState.Stunned, defendingHardness); // 防御硬直
        }
        else if (target.currentState == CharacterState.Idle)
        {
            target.SetState(CharacterState.Stunned, idleHardness); // 受击硬直
        }
        else if (target.currentState == CharacterState.AttackingStartup|| target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // 被打断硬直
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // 确反硬直
        }
    }
}

[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Hardness Effect")]
public class HardnessEffect : CardEffect
{
    public int idleHardness;     // 受击状态的硬直时间
    public int defendingHardness; // 防御时的硬直时间
    public int attackInterruptedHardness; // 被打断时的硬直时间
    public int recoveryPunishHardness; // 被确反时的硬直时间

    public override void Trigger(Character target)
    {
        ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
    }
}

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Heal Effect")]
public class HealEffect : CardEffect
{
    public int healAmount; // 治疗量

    public override void Trigger(Character target)
    {
        target.Heal(healAmount);
        Debug.Log(target.name + " is healed for " + healAmount + " health.");
    }
}
