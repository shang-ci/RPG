using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;


    public  void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stacks > 1)
            {
                itemText.text = item.stacks.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    //�������UIû�и���Inventory�仯��bug������Ϊÿ�θ���UI�Ǹ��ݿ��ĳ������ж� ���и��µ� Ȼ�������ڸ��µ�ʱ��Ҫ�ȼ�ȥ������װ�������� ���Ե��� ������©һ�� С�Ĳֿ⵼����û�취���и��� 
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
       //��ֹ�㵽�մ�����
       if (item.data.icon == null)
           return;

       //������Ʒ 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }
}
