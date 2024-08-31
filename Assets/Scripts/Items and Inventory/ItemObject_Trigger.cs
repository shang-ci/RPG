using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            //如果玩家或敌人死了 就不能再拾取
            if (collision.GetComponent<CharacterStats>().isDead)
                return;

            myItemObject.PickupItem();
        }
    }
}
