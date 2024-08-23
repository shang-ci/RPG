using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy_Skeleton enemy) : base(enemyBase, stateMachine, animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);
        rb.velocity = new Vector2(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if(enemy.IsWallDetected() || !enemy.IsGroubdDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idleState);
        }
            
    }
}
