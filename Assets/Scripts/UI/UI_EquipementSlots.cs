using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipementSlots : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot -" + slotType.ToString();
    }

    //ж��װ��
    public override void OnPointerDown(PointerEventData eventData)
    {
        //��ֹ�㵽�մ�����
        if (item.data.icon == null)
            return;

        Inventory.instance.Unequipment(item.data as ItemData_Equipment);
        Inventory.instance.AddItem(item.data as ItemData_Equipment);
        CleanUpSlot();
    }
}
