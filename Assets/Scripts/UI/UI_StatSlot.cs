using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


//���ڸ���UI������character����� stat�ۿ� 
public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string statName;
    [SerializeField]private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if(statValueText != null )
            statNameText.text = statName;
    }


    void Start()
    {
        UpdateStatValueUI();

        ui = GetComponentInParent<UI>();
    }

    //���²ۿ� ��ֵ 
    public void UpdateStatValueUI()
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        if( playerStat != null )
        {
            statValueText.text = playerStat.GetStat(statType).GetValue().ToString();

            //��������Խ����ܺ�
            if(statType == StatType.health)
                statValueText.text = playerStat.GetMaxHealthValue().ToString();

            if(statType == StatType.damage)
                statValueText.text = (playerStat.damage.GetValue() + playerStat.strength.GetValue()).ToString();

            if (statType == StatType.critPower)
                statValueText.text = (playerStat.critPower.GetValue() + playerStat.strength.GetValue()).ToString();

            if (statType == StatType.critChance)
                statValueText.text = (playerStat.critChance.GetValue() + playerStat.agility.GetValue()).ToString();

            if(statType == StatType.evasion)
                statValueText.text = (playerStat.evasion.GetValue() + playerStat.agility.GetValue()).ToString() ;

            if(statType == StatType.magicResistance)
                statValueText.text = (playerStat.magicResistance.GetValue() + (playerStat.intelligence.GetValue() * 3)).ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
