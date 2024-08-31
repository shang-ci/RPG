using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLoseMateriails;

    //���ʧȥ���е�װ���Ͳ��� 
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        //���Ǵ���һ���µ� list ���� �洢Ҫ ��ʧ��װ�� ������ֱ�Ӷ�ԭ�е�װ���������е����Է����ִ��� 
        List<InventoryItem> itemsToUnequip = new List<InventoryItem>();
        List<InventoryItem> materialsToLose = new List<InventoryItem>();

        foreach (InventoryItem item in inventory.GetEquipmentList())
        {
            if(Random.Range(0, 100) <= chanceToLooseItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }

        for (int i = 0; i < itemsToUnequip.Count; i++)
        {
            inventory.Unequipment(itemsToUnequip[i].data as ItemData_Equipment);
        }



        foreach (InventoryItem item in inventory.GetStashList())
        {
            if (Random.Range(0, 100) <= chanceToLoseMateriails)
            {
                DropItem(item.data);
                materialsToLose.Add(item);
            }
        }

        for (int i = 0; i < materialsToLose.Count; i++)
        {
            inventory.RemoveItem(materialsToLose[i].data);
        }
    }
}
