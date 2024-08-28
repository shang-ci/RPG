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

    [SerializeField] private LayerMask whatIsEnemy;//ͨ����������ͷŴ���ʱ����ˮ����������


    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExitTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }


    //�úڶ����滻������ˮ�����ѡ��Ŀ��
    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.blackHole.GetBlackholeRadius();//��������˰뾶�ĳɺڶ��뾶��һ�����
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

        //�����˶��Ϳ������˺�ը����ΧС��1ʱ��ը�����ұ�ըʱ�����ƶ�
        if (canMove)
        {
            //�޸�������Χ��û�е��˻ᱨ���bug
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

        //��ը˲����
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

    }

    //��ը����˺�
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
