using UnityEngine;

public class SkeletonIdleState : SkeletonGroundState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Skeleton enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    //ÿ�����Ͳ�ͬ�ĵ��ˣ�����skeleton�����г���rb��anim�Ȳ�ͬ�Ķ��������Ե����������



    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0f) 
            stateMachine.ChangeState(enemy.moveState);
    }
}
