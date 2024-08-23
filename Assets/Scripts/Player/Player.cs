using UnityEngine;

public class Player : Entity
{
    [Header("Move info")]
    public float moveSpeed = 8f;
    public float jumpForce = 13f;
    public float swordReturnImpact;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }


    #region states
    public PlayerStateMachine stateMachine {  get; private set; }
    public PlayeridleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlipState wallSlideState{  get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public  PlayerCatchSwordState catchSwordState { get; private set; }
    #endregion

    public SkillManager skill {  get; private set; }
    public GameObject sword {  get; private set; }


    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayeridleState(stateMachine, this, "Idle");
        moveState = new PlayerMoveState(stateMachine, this, "Move");
        jumpState = new PlayerJumpState(stateMachine, this, "Jump");
        airState = new PlayerAirState(stateMachine, this, "Jump");
        wallJumpState = new PlayerWallJumpState(stateMachine, this, "Jump");
        wallSlideState = new PlayerWallSlipState(stateMachine, this, "WallSlide");
        dashState = new PlayerDashState(stateMachine, this, "Dash");

        primaryAttack = new PlayerPrimaryAttack(stateMachine, this, "Attack");

        aimSwordState = new PlayerAimSwordState(stateMachine, this, "AimSword");
        catchSwordState = new PlayerCatchSwordState(stateMachine, this, "CatchSword");
    }

    protected override void Start()
    {
        base.Start();

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base .Update();

        stateMachine.currentState.Update();

        CheckFindDash();
    }

    public void AssignNewSword(GameObject newSword)
    {
        sword = newSword;
    }

    public void ChatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFInishTrigger();

    public void CheckFindDash()
    {
        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
                dashDir = facingDir;

            stateMachine.ChangeState(dashState);
        }
            
    }

}
