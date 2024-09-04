using UnityEngine;

//实现一个盔甲装备的效果 可以冻结敌人并且有一定的抗性来抵挡敌人的伤害 
[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item effect/Freeze enemies Effect")]
public class FreezEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    //当玩家血量低于10%的时候 敌人在攻击玩家时就会触发盔甲的效果冻结一定范围内所有的敌人 
    public override void ExecutEffect(Transform _transform)
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        if(playerStat.currentHealth > playerStat.GetMaxHealthValue() * .1f)
            return;

        if(!Inventory.instance.CanUseArmor())
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, 2);

        foreach (Collider2D collider in colliders)
        {
            collider.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
