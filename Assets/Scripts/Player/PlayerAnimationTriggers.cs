using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Enemy>() != null)
            {
                EnemyStat _target = collider.GetComponent<EnemyStat>();

                player.stats.DoDamage(_target);
               
            }
        }
    }

    //在动画中调用
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
