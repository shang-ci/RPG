using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;


    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        UpdateHealthUI();

        entity.onFlipped += FlipUI;
        myStats.onHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()//����Ѫ�����������˺�����Event����
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        
        slider.value = myStats.currentHealth;

    }

    private void FlipUI()//��UI�����Ž�ɫ��ת
    {
        myTransform.Rotate(0, 180, 0);
    }


    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        myStats.onHealthChanged -= UpdateHealthUI;
    }
}
