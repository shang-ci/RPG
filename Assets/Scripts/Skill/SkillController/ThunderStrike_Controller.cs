using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

            EnemyStat enemyTarget = collision.GetComponent<EnemyStat>();

            playerStat.DoMagicaDamage(enemyTarget);
        }
    }
}
