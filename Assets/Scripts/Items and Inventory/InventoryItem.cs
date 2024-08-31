using System;
using UnityEngine;

[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public int stacks;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();
    }

    public void AddStack() => stacks++;

    public void RemoveStack() => stacks--;
}
