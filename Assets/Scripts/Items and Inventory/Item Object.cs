using UnityEngine;


//拾取物品
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;


    //这里要注意 拾取脚本的地方 层级不能设置为item要为default不然它不能与玩家触碰并让其拾取
    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object -" + itemData.name;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }
    

    public void PickupItem()
    {
        //但这也会有一个bug 它会导致在比如说在背包里存有药品这一槽口时 但 槽口数量又已满的情况下 他无法拾取药品也就是无法叠取叠加药品的数量 
        //防止在 玩家背包满了的情况下也就是 equipment的槽口 装满了的情况下不再 把 拾取的物品给销毁 
        if(!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
