using UnityEngine;
using UnityEngine.Playables;

public class PlayerWallSlipState : PlayerState
{
    public PlayerWallSlipState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        rb.velocity = new Vector2(0, rb.velocity.y * 0.7f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }
            

        //if(xInput != 0 && player.facingDir != xInput)
           //stateMachine.ChangeState(player.idleState); 
        
        if(player.IsGroubdDetected())
            stateMachine.ChangeState(player.idleState);

        if (yInput < 0)
            rb.velocity = new Vector2(0, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y * .7f);

    }
}
