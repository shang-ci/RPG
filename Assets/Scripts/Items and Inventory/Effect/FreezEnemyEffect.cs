using UnityEngine;

//ʵ��һ������װ����Ч�� ���Զ�����˲�����һ���Ŀ������ֵ����˵��˺� 
[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item effect/Freeze enemies Effect")]
public class FreezEnemyEffect : ItemEffect
{
    [SerializeField] private float duration;

    //�����Ѫ������10%��ʱ�� �����ڹ������ʱ�ͻᴥ�����׵�Ч������һ����Χ�����еĵ��� 
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
