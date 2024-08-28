using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private SpriteRenderer sr;
    [SerializeField] private float colorLoosingSpeed;//������ʧʱ�� 
    [SerializeField] private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private Transform closestEnemy;

    private int facingDir = 1;//�����Ŀ�¡���λ�����ڵ��˱��󡪡����������canDuplicateClone

    private bool canDuplicateClone;//�ÿ�¡�м����ٴ����ɿ�¡��
    private float chanceToDuplicate;//����


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

    public void SetupClone(Transform newTransform, float cloneDuration, bool canAttack, Vector3 _offset, Transform _closestEnemy,bool _canDuplicateClone, float _chanceToDuplicate)
    {
        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));

        transform.position = newTransform.position + _offset;
        cloneTimer = cloneDuration;

        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;
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
            {
                if(player == null)
                Debug.Log("player null");
                PlayerManger.instance.player.stats.DoDamage(collider.GetComponent<CharacterStats>());
            }


            if (canDuplicateClone)
            {
                //�������ɿ�¡��
                if(Random.Range(0,100) < chanceToDuplicate)
                {
                    SkillManager.instance.clone.CreateClone(collider.transform, new Vector3(1.5f * facingDir, 0));
                }
            }
        }
    }

    private void FaceClosestTarget()
    {
        if(closestEnemy != null && transform.position.x > closestEnemy.position.x)
        {
            facingDir = -1;
            transform.Rotate(0, 180, 0);
        }
    }
}
