using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType 
{
    Weapon,//����
    Armor,//����
    Amlet,//�����
    Flask//
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    //ÿ����Ʒ�����в�ͬ��Ч������ �������� �������˵ȵȵȵ� 
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength; // ���� ����1�� �������� 1% �￹
    public int agility;// ���� ���� 1% ���ܼ������� 1%
    public int intelligence;// 1 �� ħ���˺� 1��ħ�� 
    public int vitality;//��Ѫ��

    [Header("Offensive stats")]
    public int damage;
    public int critChance;      // ������
    public int critPower;       //150% ����

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;//����ֵ
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Graft requirements")]
    public List<InventoryItem> craftingMaterials;


    //ʵ��װ����Ч�� 
    public void Effect(Transform _enemyPosition)
    {
        foreach(var item  in itemEffects)
        {
            item.ExecutEffect(_enemyPosition);
        }
    }


    public void AddModifiers()
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        playerStat.maxHealth.AddModifier(maxHealth);
        playerStat.armor.AddModifier(armor);
        playerStat.evasion.AddModifier(evasion);
        playerStat.magicResistance.AddModifier(magicResistance);

        playerStat.fireDamage.AddModifier(fireDamage);
        playerStat.iceDamage.AddModifier(iceDamage);
        playerStat.lightingDamage.AddModifier(lightingDamage);

        playerStat.damage.AddModifier(damage);
        playerStat.critChance.AddModifier(critChance);
        playerStat.critPower.AddModifier(critPower);

        playerStat.strength.AddModifier(strength);
        playerStat.agility.AddModifier(agility);
        playerStat.intelligence.AddModifier(intelligence);
        playerStat.vitality.AddModifier(vitality);


    }

    public void RemoveModifiers() 
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        playerStat.maxHealth.RemoveModifier(maxHealth);
        playerStat.armor.RemoveModifier(armor);
        playerStat.evasion.RemoveModifier(evasion);
        playerStat.magicResistance.RemoveModifier(magicResistance);

        playerStat.fireDamage.RemoveModifier(fireDamage);
        playerStat.iceDamage.RemoveModifier(iceDamage);
        playerStat.lightingDamage.RemoveModifier(lightingDamage);

        playerStat.damage.RemoveModifier(damage);
        playerStat.critChance.RemoveModifier(critChance);
        playerStat.critPower.RemoveModifier(critPower);

        playerStat.strength.RemoveModifier(strength);
        playerStat.agility.RemoveModifier(agility);
        playerStat.intelligence.RemoveModifier(intelligence);
        playerStat.vitality.RemoveModifier(vitality);
    }
}
