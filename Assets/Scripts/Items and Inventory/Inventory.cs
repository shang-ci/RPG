using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> satrtingItems;//��ҵĳ�ʼװ������Դ 

    //���װ������Ҹ�������
    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;//����
    public Dictionary<ItemData, InventoryItem> inventoryDictianory;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;//���� ״̬�ۿ� 


    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_EquipementSlots[] equipmentSlot;
    private UI_StatSlot[] statSlot;

    [Header("Item Cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;
    //����Щ����ȴ��װ����������Ϸһ��ʼ��װ��װ��ʱ�Ϳ���ʹ�����ǵļ���Ч�� 
    private float flaskCooldown;//��Ѫ������ȴʱ�� 
    private float armorCooldown;//���Զ�����˵Ŀ��׵���ȴʱ�� 


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
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    //��ҳ�ʼ���� ������ 
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
          //����Ӧ���͵����������Ӧ�Ĳ���
          foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
          {
              if (item.Key.equipmentType == equipmentSlot[i].slotType)
              {
                  equipmentSlot[i].UpdateSlot(item.Value);
              }
          }
      
      }

        //�������UIû�и���Inventory�仯��bug
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

        for( int i = 0; i < statSlot.Length ; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void EquipItem(ItemData _item)
  {
      //�����itemdata���ò�������equipment���enum������
      ItemData_Equipment newEquipment = _item as ItemData_Equipment;
      //������ת��Ϊ����
      InventoryItem newItem = new InventoryItem(newEquipment);
  
      ItemData_Equipment oldEquipment = null;
  
      foreach (KeyValuePair<ItemData_Equipment, InventoryItem> item in equipmentDictionary)
      {
          if (item.Key.equipmentType == newEquipment.equipmentType)
          {
              oldEquipment = item.Key;//��key�������ⲿ��data������
          }
      }//������foreach���value��key�޷����ⲿ��list���ֵ���в���
  
      if (oldEquipment != null)
      {
          Unequipment(oldEquipment);
          AddItem(oldEquipment);
      }
  
      equipment.Add(newItem);
      equipmentDictionary.Add(newEquipment, newItem);
      RemoveItem(_item);
  
      //�������
      newEquipment.AddModifiers();

      UpdateSlotUI();
  }

    //װ������ͬ���͵�װ��ʱ,ȥ����װ����װ��
    public void Unequipment(ItemData_Equipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();//�Ƴ��ӳ�
        }
    }

    //���ڷ�ֹ ��Ʒ���������ۿ� ���� ����������Χ 
    public bool CanAddItem()
    {
        if(inventory.Count >= inventoryItemSlot.Length)
        {
            return false;
        }

        return true;
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotUI();
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictianory.TryGetValue(_item, out InventoryItem value))//ֻ�����ַ��������ڲ��ҵ��Ƿ����key��Ӧvalue�Ƿ���ڵ�ͬʱ���ܹ�ͬʱ�õ�value�������������ò���value
        {
            value.AddStack();
        }//�ֵ��ʹ�ã�ͨ��ItemData���͵������ҵ�InventoryItem�����֮��Ӧ��ͬ�����͵�����
        else//��ʼʱ����û����ͬ���͵����壬�ʵ���else��Ϊ�˳�ʼ����棬ʹ���к���һ��������ֵ
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);//����б���ֻ��һ��
            inventoryDictianory.Add(_item, newItem);//ͬ��
        }
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))//ֻ�����ַ��������ڲ��ҵ��Ƿ����key��Ӧvalue�Ƿ���ڵ�ͬʱ���ܹ�ͬʱ�õ�value�������������ò���value
        {
            value.AddStack();
        }//�ֵ��ʹ�ã�ͨ��ItemData���͵������ҵ�InventoryItem�����֮��Ӧ��ͬ�����͵�����
        else//��ʼʱ����û����ͬ���͵����壬�ʵ���else��Ϊ�˳�ʼ����棬ʹ���к���һ��������ֵ
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);//����б���ֻ��һ��
            stashDictionary.Add(_item, newItem);//ͬ��
        }
    }


    public void RemoveItem(ItemData _item)
    {
        //��������Ҷ����һ������һֱû�ҵ�����ͳ���������
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


    //�ϳ�װ��
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


    //�����������ʱ��ʧװ�� ����ȡ������е�װ����Ϣ 
    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem> GetStashList() => stash;


    //����װ���е��κ���Ʒ ����ʵ��ÿ��װ�����Դ�Ч�� 
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

    //��Ѫ��
    public void UseFlask()
    {
        ItemData_Equipment currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null) 
            return;

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null);
            lastTimeUsedFlask = Time.time;
        }
        else
            Debug.Log("flask on colldown");

    }


    //����װ�� ��ʹ�� 
    public bool CanUseArmor()
    {
        ItemData_Equipment currentArmor = GetEquipment(EquipmentType.Armor);

        if(Time.time > lastTimeUsedArmor + armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor = Time.time;
            return true;
        }

        Debug.Log("Armor on colldown");
        return false;
    }
}
