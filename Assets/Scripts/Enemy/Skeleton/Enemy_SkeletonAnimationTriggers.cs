using UnityEngine;

public class Enemy_SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationFinishTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.GetComponent<Player>() != null)
                collider.GetComponent<Player>().Damage();
        }
    }

    private void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();
}
