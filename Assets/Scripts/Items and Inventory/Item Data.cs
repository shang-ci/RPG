using System.Text;
using UnityEngine;

public enum ItemType
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;//图标


    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    //构建 字符串内容 ——在每个不同的装备上 返回不同的描述 
    public virtual string GetDescription()
    {
        return "";
    }
}