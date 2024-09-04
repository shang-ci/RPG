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
    public Sprite icon;//ͼ��


    [Range(0, 100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();

    //���� �ַ������� ������ÿ����ͬ��װ���� ���ز�ͬ������ 
    public virtual string GetDescription()
    {
        return "";
    }
}