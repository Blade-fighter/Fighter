using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[System.Serializable]
public class StepMoveData
{
    public Vector2 moveVector; // 移动的向量，包含方向和距离
    public int ke; // 移动的刻数
}

[CreateAssetMenu(fileName = "New Complex Move Effect", menuName = "Card Effects/Fight/Complex Move Effect")]
public class ComplexMoveEffect : CardEffect // 移动效果
{
    [Header("纯移动技能把这个设置成true")]
    public bool setState = false;//纯移动技能把这个设置成true

    public List<StepMoveData> moveSteps; // 存储每一步的移动信息
    public bool giveToTarget;//是否是给敌人的，否为给自己
    public bool toGround;//打完是否回地面
    public override void Trigger(Character target, Character attacker)
    {
        List <StepMoveData> tempSteps = moveSteps;
        if (setState)
        {
            int attackerMoveKe= 0;
            foreach (StepMoveData step in tempSteps)
            {
                attackerMoveKe += step.ke;
            }
            attacker.SetState(CharacterState.MovingFront, attackerMoveKe);
        }
        //设置的时候默认出招者在左敌人在右，对敌方的效果正数为往右
        if ((giveToTarget&&!target.dir)||(!giveToTarget&&attacker.dir))
        {
            foreach (StepMoveData step in tempSteps) //反向
            {
                step.moveVector = new Vector2(-step.moveVector.x, step.moveVector.y);
            }
        }
        if(giveToTarget)
        ApplyMultiStepMove(target, tempSteps, toGround);
        if(!giveToTarget)
        ApplyMultiStepMove(attacker, tempSteps, toGround);
    }

}