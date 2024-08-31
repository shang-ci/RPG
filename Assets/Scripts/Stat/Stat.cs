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

        //�ṩһ�ֿ��Ա仯�˺��ķ�ʽ
        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;//���ú�������ֵ�ķ�ʽ�ṩ����
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
