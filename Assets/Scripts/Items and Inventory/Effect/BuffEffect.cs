using UnityEngine;

//�������� ��������Ч�� װ�����Ӹ������� 
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
