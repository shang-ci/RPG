using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTimer = .3f;//Ӱ����ҵ��ĸ߶�
    private bool skillUsed;
    private float defaultGravity;


    public PlayerBlackHoleState(PlayerStateMachine stateMachine, Player player, string animBoolName) : base(stateMachine, player, animBoolName)
    {
    }

    public override void AnimationFInishTrigger()
    {
        base.AnimationFInishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTimer;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransprent(false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.5f);

            if(!skillUsed)
            {
                if( player.skill.blackHole.CanUseSkill())
                    skillUsed = true;
            }
        }

        //���������뿪�ķ�ʽ
        if (player.skill.blackHole.SkillCompleteed())
            stateMachine.ChangeState(player.airState);
    }
}
