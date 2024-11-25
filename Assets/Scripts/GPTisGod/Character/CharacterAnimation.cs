using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    public Character character;
    private int hitAnimationIndex = 0; // ����˳�򲥷��ܻ�����
    private void Start()
    {
        character=GetComponent<Character>();
    }
    public void BasicAnimationController(CharacterState state)
    {
        ResetAllTrigger();
        //trigger�߼�
        switch (state)//����״̬������
        {
            case CharacterState.Idle:
                character.animator.SetTrigger("Idle");
                break;
            case CharacterState.Defending:
                character.animator.SetTrigger("Defend");
                break;
            case CharacterState.Stunned:
                PlayRandomHitAnimation();
                break;
            case CharacterState.Airborne:
                character.animator.SetTrigger("Airborne");
                break;
            case CharacterState.AirborneAttacked:
                character.animator.SetTrigger("AirborneAttacked");
                break;
            case CharacterState.Downed:
                character.animator.SetTrigger("ToDown");
                break;
            case CharacterState.Thrown:
                character.animator.SetTrigger("Thrown");
                break;
            // �������״̬���߼�
            default:
                character.animator.SetTrigger("Idle");
                break;
        }
                /*//bool�߼�
                switch (state)//����״̬������
                {
                    case CharacterState.Idle:
                        OneToTrue("isIdle");
                        break;
                    case CharacterState.Defending:
                        OneToTrue("isDefending");
                        break;
                    case CharacterState.Stunned:
                        OneToTrue("isStunned");
                        break;
                    case CharacterState.Airborne:
                        OneToTrue("isAirborne");
                        break;
                    case CharacterState.AirborneAttacked:
                        OneToTrue("isAirborneAttacked");
                        break;
                    case CharacterState.Downed:
                        OneToTrue("isDowned");
                        break;
                    case CharacterState.Thrown:
                        OneToTrue("isThrown");
                        break;
                    // �������״̬���߼�
                    default:
                        OneToTrue(null);
                        break;
                }*/
        }
    public void OneToTrue(string name)//��������ȡһ��boolΪtrue����Ϊfalse�ɷ���
    {
        character.animator.SetBool("isIdle", false);
        character.animator.SetBool("isDefending", false);
        character.animator.SetBool("isStunned", false);
        character.animator.SetBool("isAirborne", false);
        character.animator.SetBool("isAirborneAttacked", false);
        character.animator.SetBool("isDowned", false);
        character.animator.SetBool("isThrown", false);
        if (name != null)
        character.animator.SetBool(name, true);
    }
    void ResetAllTrigger()//��������trigger
    {
        character.animator.ResetTrigger("Idle");
        character.animator.ResetTrigger("Defend");
        character.animator.ResetTrigger("Airborne");
        character.animator.ResetTrigger("AirborneAttacked");
        character.animator.ResetTrigger("ToDown");
        character.animator.ResetTrigger("Thrown");
        character.animator.ResetTrigger("Stun1"); 
        character.animator.ResetTrigger("Stun2"); 
        character.animator.ResetTrigger("Stun3");
    }
    // ˳�򲥷��ܻ�����
    public void PlayNextHitAnimation()
    {
        hitAnimationIndex = (hitAnimationIndex % 3) + 1; // ѭ�� 1, 2, 3

        switch (hitAnimationIndex)
        {
            case 1:
                character.animator.SetTrigger("Stun1");
                break;
            case 2:
                character.animator.SetTrigger("Stun2");
                break;
            case 3:
                character.animator.SetTrigger("Stun3");
                break;
        }
    }

    // ��������ܻ�����
    public void PlayRandomHitAnimation()
    {
        int randomIndex = Random.Range(1, 4); // ��� 1, 2, 3

        switch (randomIndex)
        {
            case 1:
                character.animator.SetTrigger("Stun1");
                break;
            case 2:
                character.animator.SetTrigger("Stun2");
                break;
            case 3:
                character.animator.SetTrigger("Stun3");
                break;
        }
    }
}
