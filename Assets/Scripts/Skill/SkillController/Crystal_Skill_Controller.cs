using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();
    private Player player;

    private float crystalExitTimer;

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;

    private Transform closestTarget;

    [SerializeField] private LayerMask whatIsEnemy;//通过这个，在释放大招时生成水晶攻击敌人


    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }


    //让黑洞里替换出来的水晶随机选择目标
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackholeRadius();//把随机敌人半径改成黑洞半径的一半就行
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 50, whatIsEnemy);

        if (colliders.Length >= 0)
        {
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
        }
    }
    private void Update()
    {
        crystalExitTimer -= Time.deltaTime;
        if (crystalExitTimer < 0)
        {
            FinishCrystal();
        }

        //可以运动就靠近敌人后爆炸，范围小于1时爆炸，并且爆炸时不能移动
        if (canMove)
        {
            //修复攻击范围内没有敌人会报错的bug
            if (closestTarget != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, closestTarget.position) < 1)
                {
                    FinishCrystal();
                    canMove = false;
                }
            }
            else
                transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(5, 0, 0), moveSpeed * Time.deltaTime);

        }

        //爆炸瞬间变大
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

    }

    //爆炸造成伤害
    public void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                player.stats.DoMagicaDamage(hit.GetComponent<CharacterStats>());
        }
    }

    public void FinishCrystal()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetBool("Explode", true);
        }
        else
        {
            SelfDestory();
        }
    }

    public void SelfDestory() => Destroy(gameObject);
}
