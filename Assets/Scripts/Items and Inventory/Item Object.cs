using UnityEngine;


//ʰȡ��Ʒ
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;


    //����Ҫע�� ʰȡ�ű��ĵط� �㼶��������ΪitemҪΪdefault��Ȼ����������Ҵ���������ʰȡ
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
        //����Ҳ����һ��bug ���ᵼ���ڱ���˵�ڱ��������ҩƷ��һ�ۿ�ʱ �� �ۿ������������������ ���޷�ʰȡҩƷҲ�����޷���ȡ����ҩƷ������ 
        //��ֹ�� ��ұ������˵������Ҳ���� equipment�Ĳۿ� װ���˵�����²��� �� ʰȡ����Ʒ������ 
        if(!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
