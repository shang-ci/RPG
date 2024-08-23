using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clone_Skill_Controller�ǰ���Clone���ϵ�Ҳ����Prefah�ϵ�
public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private Animator anim;//����animator
    private SpriteRenderer sr;//����Sr
    [SerializeField] private float colorLoosingSpeed;//������ʧʱ�� 
    [SerializeField] private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;

    private int facingDir = 1;//����ǿ���λ�õģ������Ŀ�¡���λ�����ڵ������

    private bool canDuplicateClone;
    private float chanceToDuplicate;


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1,sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if(sr.color.a <= 0 )
                Destroy(gameObject);
        }
    }

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack)
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        transform.position = newTransform.position;
        cloneTimer = cloneDuration;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
                collider.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosestTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if(distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }
        }

        if(closestEnemy != null && transform.position.x > closestEnemy.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
    }
}
