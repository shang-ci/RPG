using UnityEngine;


[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecutEffect(Transform _enemyPosition)
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        //计算回血的血量通过 玩家自身最大血量的百分比计算 
        int healAmount = Mathf.RoundToInt(playerStat.GetMaxHealthValue() * healPercent);
   
        playerStat.IncreaseHealthBy(healAmount);
    }
}
