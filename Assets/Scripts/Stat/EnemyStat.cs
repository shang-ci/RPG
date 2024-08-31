using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;//���ܵ��˵�����Ʒ

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percantageModifier = 0.2f;



    protected override void Start()
    {
        ApplyLevelModifiers();//��start֮ǰ���У���Ѫ������

        base.Start();

        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
    }

    //�ȼ���������
    private void ApplyLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);

        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);
    }

    private void Modify(Stat _stat)
    {
        for(int i = 1; i <= level; i++)
        {
            float modifier = _stat.GetValue() * percantageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();

        //���������Ʒ
        myDropSystem.GenerateDrop();
    }
}
