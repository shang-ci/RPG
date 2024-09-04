using UnityEngine;


[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item effect/Heal Effect")]
public class HealEffect : ItemEffect
{
    [Range(0, 1f)]
    [SerializeField] private float healPercent;

    public override void ExecutEffect(Transform _enemyPosition)
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        //�����Ѫ��Ѫ��ͨ�� ����������Ѫ���İٷֱȼ��� 
        int healAmount = Mathf.RoundToInt(playerStat.GetMaxHealthValue() * healPercent);
   
        playerStat.IncreaseHealthBy(healAmount);
    }
}
