using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> satrtingItems;//玩家的初始装备和资源 

    //点击装备给玩家附上属性
    public List<InventoryItem> equipment;//inventoryItems类型的列表
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;//以ItemData为Key寻找InventoryItem的字典

    public List<InventoryItem> inventory;//管理
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;


    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipementSlots[] equipmentSlot;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventoryDictianory = new Dictionary<ItemData, InventoryItem>();
        inventory = new List<InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipementSlots>();

        AddStartingItems();
    }

    //玩家初始背包 的内容 
    private void AddStartingItems()
    {
        for (int i = 0; i < satrtingItems.Count; i++)
        {
            AddItem(satrtingItems[i]);
        }
    }

    private void UpdateSlotUI()
    {
      for (int i = 0; i < equipmentSlot.Length; i++)
      {
          //将对应类型的武器插入对应的槽内
          foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
          {
              if (item.Key.equipmentType == equipmentSlot[i].slotType)
              {
                  equipmentSlot[i].UpdateSlot(item.Value);
              }
          }
      
      }

        //解决出现UI没有跟着Inventory变化的bug
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i <inventory.Count ; i++)
        { 
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }

        for(int i = 0;i <stash.Count ; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }
    }

    public void EquipItem(ItemData _item)
  {
      //解决在itemdata里拿不到子类equipment里的enum的问题
      ItemData_Equipment newEquipment = _item as ItemData_Equipment;//https://www.bilibili.com/read/cv15551811/
      //将父类转换为子类
      InventoryItem newItem = new InventoryItem(newEquipment);
  
      ItemData_Equipment oldEquipment = null;
  
      foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)//这种方法可以同时拿到key和value保存到item里面
      {
          if (item.Key.equipmentType == newEquipment.equipmentType)//将拿到的key与转换成itemdata_equipment类型的_item的type对比拿到存在的key
          {
              oldEquipment = item.Key;//此key需保存在外部的data类型里
          }
      }//好像用foreach里的value和key无法对外部的list和字典进行操作
  
      if (oldEquipment != null)
      {
          Unequipment(oldEquipment);
          AddItem(oldEquipment);
      }
  
      equipment.Add(newItem);
      equipmentDictionary.Add(newEquipment, newItem);
      RemoveItem(_item);
  
      //添加属性
      newEquipment.AddModifiers();

      UpdateSlotUI();
  }

    //装备其他同类型的装备时,去除已装备的装备
    public void Unequipment(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();//移出加成
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment)
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//只有这种方法才能在查找到是否存在key对应value是否存在的同时，能够同时拿到value，其他方法的拿不到value
        {
            value.AddStack();
        }//字典的使用，通过ItemData类型的数据找到InventoryItem里的与之对应的同样类型的数据
        else//初始时由于没有相同类型的物体，故调用else是为了初始化库存，使其中含有一个基本的值
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);//填进列表里只有一次
            inventoryDictianory.Add(_item, newItem);//同上
        }
    }

    private void AddToStash(ItemData _item)//向stash加物体的函数
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//只有这种方法才能在查找到是否存在key对应value是否存在的同时，能够同时拿到value，其他方法的拿不到value
        {
            value.AddStack();
        }//字典的使用，通过ItemData类型的数据找到InventoryItem里的与之对应的同样类型的数据
        else//初始时由于没有相同类型的物体，故调用else是为了初始化库存，使其中含有一个基本的值
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);//填进列表里只有一次
            stashDictionary.Add(_item, newItem);//同上
        }
    }


    public void RemoveItem(ItemData _item)
    {
        //就是这里，我多加了一个！，一直没找到错误就出现在这里
        if(inventoryDictianory.TryGetValue(_item,out InventoryItem value))
        {
            if(value.stacks <= 1)
            {
                inventory.Remove(value);
                inventoryDictianory.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            if (stashValue.stacks <= 1)
            {
                stash.Remove(value);
                stashDictionary.Remove(_item);

            }
            else
                stashValue.RemoveStack();
        }

        UpdateSlotUI();
    }


    //合成装备
    public bool CanCraft(ItemData_Equipment _itemToCraft, List<InventoryItem> _requiredMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for(int i = 0; i < _requiredMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requiredMaterials[i].data,out InventoryItem stashValue))
            {
                if((stashValue.stacks < _requiredMaterials[i].stacks))
                {
                    Debug.Log("no enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("no enough materials");
                return false;
            }
        }

        for(int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }

        AddItem(_itemToCraft);
        Debug.Log("here is your item" + _itemToCraft.name);

        return true;
    }


    //用于玩家死亡时丢失装备 ，获取玩家已有的装备信息 
    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;


    //访问装备中的任何物品 用来实现每个装备的自带效果 
    public ItemData_Equipment GetEquipment(EquipmentType _type)
    {
        ItemData_Equipment equipedItem = null;

        foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
                equipedItem = item.Key;
        }

        return equipedItem;
    }
}
