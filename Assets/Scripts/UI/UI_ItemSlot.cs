using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    private UI ui;
    public InventoryItem item;


    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

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

    //解决出现UI没有跟着Inventory变化的bug——因为每次更新UI是根据库存的长度来判断 进行更新的 然而我们在更新的时候要先减去我们已装备的武器 所以导致 最后会是漏一个 小的仓库导致他没办法进行更新 
    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
       //防止点到空处报错
       if (item == null)
           return;

       //售卖物品 
        if (Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null) 
            return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
            return;

        ui.itemToolTip.HideToolTip();
    }
}
