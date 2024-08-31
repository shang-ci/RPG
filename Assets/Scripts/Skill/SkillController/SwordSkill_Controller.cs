using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwordSkill_Controller : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;//收回sword

    private float returnSpeed = 12;
    private float freezeTimeDuration;//定住的时间

    [Header("Pierce info")]
    [SerializeField] private float pierceAmount;


    [Header("Bounce info")]
    private float bounceeSpeed;
    private bool isBounce;
    private int bounceAmount;
    private List<Transform> enemyTraget;
    private int tragetIndex;

    [Header("Spin info")]
    private float maxTraveIDistance;//距离
    private float spinDuration;//持续时间
    private float spinTimer;
    private bool wassStopped;//是否定留
    private bool isSpinning;


    private float hitTimer;
    private float hitCoolDown;//攻击频率
    private float spinDirection;//持续方前缓慢移动


    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();  
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.ChatchTheSword();
        }

        BounceLogic();

        SpinLogic();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTraveIDistance && !wassStopped)
            {
                StopWhenSpinning();
            }

            if (wassStopped)
            {
                spinTimer -= Time.deltaTime;

                //持续向前旋转
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);

                if (spinTimer < 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCoolDown;

                    Collider2D[] colluders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colluders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamge(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        wassStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isBounce && enemyTraget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTraget[tragetIndex].position, bounceeSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, enemyTraget[tragetIndex].position) < .1f)
            {
                //冻住敌人在trigger检测时，不是每个类型的sword都可以冻结吗，，还要在重复加
                SwordSkillDamge(enemyTraget[tragetIndex].GetComponent<Enemy>());

                tragetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                {
                    isBounce = false;
                    isReturning = true;
                }

                if (tragetIndex >= enemyTraget.Count)
                    tragetIndex = 0;
            }
        }
    }

    public void SetupSword(Vector2 dir, float gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        rb.velocity = dir;
        rb.gravityScale = gravityScale;

        if(pierceAmount <= 0)
            anim.SetBool("Rotation",true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);

        Invoke("DestroyMe", 6);
    }

    public void SetuopPierce(int _pierceAmount)
    {
        pierceAmount = _pierceAmount;
    }

    public void SetupBounce(bool _isBounce, int _amountOfBounce, float _bounceSpeed)
    {
        isBounce = _isBounce;
        bounceAmount = _amountOfBounce;
        bounceeSpeed = _bounceSpeed;
        enemyTraget = new List<Transform>();
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTraveIDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
    }


    public void ReturnSword()
    {
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning =true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamge(enemy);
        }

        SetupTargetsForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamge(Enemy enemy)
    {
        // enemy.DamageEffect();
        //改变攻击方式，物理和三种法术攻击
        PlayerManger.instance.player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.StartCoroutine("FreezeeTimerFor", freezeTimeDuration);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBounce && enemyTraget.Count <= 0)
            {
                Collider2D[] colluders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colluders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTraget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if(pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }

        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }


        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBounce && enemyTraget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
