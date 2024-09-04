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

                if (_target != null)
                    player.stats.DoDamage(_target);

                //执行物品的效果 ,,防止在获得 拥有该效果的武器之前就调用这个装备导致为空 
                Inventory.instance.GetEquipment(EquipmentType.Weapon)?.Effect(_target.transform);
               
            }
        }
    }

    //在动画中调用
    private void ThrowSword()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
