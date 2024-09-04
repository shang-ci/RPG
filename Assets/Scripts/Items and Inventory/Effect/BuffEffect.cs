using UnityEngine;

//用来整理 各种增益效果 装备增加各种属性 
[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Item effect/Buff Effect")]
public class BuffEffect : ItemEffect
{
    private PlayerStat stat;
    [SerializeField] StatType buffType;
    [SerializeField] private int buffAmount;
    [SerializeField] private int buffDuration;


    public override void ExecutEffect(Transform _enemyPosition)
    {
        stat = PlayerManger.instance.player.GetComponent<PlayerStat>();
        stat.IncreaseStatBy(buffAmount, buffDuration, stat.GetStat(buffType));
    }


}
