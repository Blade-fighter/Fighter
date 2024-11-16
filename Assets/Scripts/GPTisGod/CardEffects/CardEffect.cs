using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Trigger(Character target);

    // Ӧ��ӲֱЧ��
    public void ApplyHardness(Character target, int idleHardness, int defendingHardness, int attackInterruptedHardness, int recoveryPunishHardness)
    {
        if (target.currentState == CharacterState.Defending)
        {
            target.SetState(CharacterState.Stunned, defendingHardness); // ����Ӳֱ
        }
        else if (target.currentState == CharacterState.Idle)
        {
            target.SetState(CharacterState.Stunned, idleHardness); // �ܻ�Ӳֱ
        }
        else if (target.currentState == CharacterState.AttackingStartup|| target.currentState == CharacterState.AttackingActive)
        {
            target.SetState(CharacterState.Stunned, attackInterruptedHardness); // �����Ӳֱ
        }
        else if (target.currentState == CharacterState.Recovery)
        {
            target.SetState(CharacterState.Stunned, recoveryPunishHardness); // ȷ��Ӳֱ
        }
    }
}

[CreateAssetMenu(fileName = "New Hardness Effect", menuName = "Card Effects/Hardness Effect")]
public class HardnessEffect : CardEffect
{
    public int idleHardness;     // �ܻ�״̬��Ӳֱʱ��
    public int defendingHardness; // ����ʱ��Ӳֱʱ��
    public int attackInterruptedHardness; // �����ʱ��Ӳֱʱ��
    public int recoveryPunishHardness; // ��ȷ��ʱ��Ӳֱʱ��

    public override void Trigger(Character target)
    {
        ApplyHardness(target, idleHardness, defendingHardness, attackInterruptedHardness, recoveryPunishHardness);
    }
}

[CreateAssetMenu(fileName = "New Heal Effect", menuName = "Card Effects/Heal Effect")]
public class HealEffect : CardEffect
{
    public int healAmount; // ������

    public override void Trigger(Character target)
    {
        target.Heal(healAmount);
        Debug.Log(target.name + " is healed for " + healAmount + " health.");
    }
}
