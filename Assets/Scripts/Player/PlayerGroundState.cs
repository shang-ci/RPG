using UnityEngine;
using UnityEngine.Playables;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
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

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackHoleState);

        if(Input.GetKeyDown(KeyCode.Mouse1) && HashNoSword())
            stateMachine.ChangeState(player.aimSwordState);

        if(Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttack);

        if(!player.IsGroubdDetected())
           stateMachine.ChangeState(player.airState);

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroubdDetected()) 
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HashNoSword()
    {
        if(!player.sword)
            return true;

        player.sword.GetComponent<SwordSkill_Controller>().ReturnSword();
        return false;
    }
}
