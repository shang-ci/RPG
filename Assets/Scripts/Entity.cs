using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    protected bool isKnocked;

    [Header("collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    #region components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    #endregion

    public System.Action onFlipped;

    protected virtual void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        spriteRenderer  = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();//不需要再孩子上获得
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage, float flowDuration)//减缓一切速度函数
    {

    }

    protected virtual void ReturnDefaultSpeed()//动画速度恢复正常函数
    {
        anim.speed = 1;
    }


    public virtual void DamageImpack()
    {
        StartCoroutine("HitKnockback");
        //Debug.Log(gameObject.name + "damage");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;

        //改成相对方向会更好
        rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(.3f);

        isKnocked = false;
    }

    public virtual bool IsGroubdDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
    }

    public void Flip()
    {
        //防止enemy被击飞时检测饿不到地面，进行翻转
        if (isKnocked)
            return;

        facingDir = facingDir * -1;
        facingRight = !facingRight;
        //transform.Rotate(0, 180, 0);
        Vector3 characterScale = transform.localScale;
        characterScale.x *= -1;
        transform.localScale = characterScale;

        if (onFlipped != null)
            onFlipped();
    }

    public void FlipController(float x)
    {
        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }
    }

    public virtual void SetZeroVelocity()
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(0, 0);
    }

    public virtual void SetVelocity(float xvelocity, float yvelocity)
    {
        if(isKnocked)
            return;

        rb.velocity = new Vector2(xvelocity, yvelocity);
        FlipController(xvelocity);
    }

    public virtual void Die()
    {

    }
}
