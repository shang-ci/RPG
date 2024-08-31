using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType 
{
    Weapon,//武器
    Armor,//盔甲
    Amlet,//护身符
    Flask//
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    //每个物品都带有不同的效果比如 冰冻闪电 减速增伤等等等等 
    public ItemEffect[] itemEffects;

    [Header("Major stats")]
    public int strength; // 力量 增伤1点 爆伤增加 1% 物抗
    public int agility;// 敏捷 闪避 1% 闪避几率增加 1%
    public int intelligence;// 1 点 魔法伤害 1点魔抗 
    public int vitality;//加血的

    [Header("Offensive stats")]
    public int damage;
    public int critChance;      // 暴击率
    public int critPower;       //150% 爆伤

    [Header("Defensive stats")]
    public int maxHealth;
    public int armor;
    public int evasion;//闪避值
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Graft requirements")]
    public List<InventoryItem> craftingMaterials;


    //实现装备的效果 
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
