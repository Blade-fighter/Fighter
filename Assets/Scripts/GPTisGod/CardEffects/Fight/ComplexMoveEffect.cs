using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class StepMoveData
{
    public Vector2 moveVector; // �ƶ�����������������;���
    public int ke; // �ƶ��Ŀ���
}

[CreateAssetMenu(fileName = "New Complex Move Effect", menuName = "Card Effects/Fight/Complex Move Effect")]
public class ComplexMoveEffect : CardEffect // �ƶ�Ч��
{
    [Header("���ƶ����ܰ�������ó�true")]
    public bool setState = false;//���ƶ����ܰ�������ó�true

    public List<StepMoveData> moveSteps; // �洢ÿһ�����ƶ���Ϣ
    public bool giveToTarget;//�Ƿ��Ǹ����˵ģ���Ϊ���Լ�
    public bool toGround;//�����Ƿ�ص���
    public override void Trigger(Character target, Character attacker)
    {
        if (setState)
        {
            int attackerMoveKe= 0;
            foreach (StepMoveData step in moveSteps)
            {
                attackerMoveKe += step.ke;
            }
            attacker.SetState(CharacterState.MovingFront, attackerMoveKe);
        }
        //���õ�ʱ��Ĭ�ϳ���������������ң��Եз���Ч������Ϊ����
        if ((giveToTarget&&!target.dir)||(!giveToTarget&&attacker.dir))
        {
            foreach (StepMoveData step in moveSteps) //����
            {
                step.moveVector = new Vector2(-step.moveVector.x, step.moveVector.y);
            }
        }
        if(giveToTarget)
        ApplyMultiStepMove(target,moveSteps,toGround);
        if(!giveToTarget)
        ApplyMultiStepMove(attacker,moveSteps,toGround);
    }

}