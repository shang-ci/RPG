using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat 
{

    [SerializeField]private int baseValue;

    public List<int> modifiers;

    

    public int GetValue()
    {
        int finalValue = baseValue;

        //提供一种可以变化伤害的方式
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;//采用函数返回值的方式提供数据
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
