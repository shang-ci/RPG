using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [SerializeField] private float chanceToLooseItems;
    [SerializeField] private float chanceToLoseMateriails;

    //随机失去已有的装备和材料 
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;

        //我们创建一个新的 list 用来 存储要 丢失的装备 而不是直接对原有的装备库来进行调整以防出现错误 
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
