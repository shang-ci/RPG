using TMPro;
using UnityEngine;


//���ڸ���UI������character����� stat�ۿ� 
public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;
    [SerializeField]private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if(statValueText != null )
            statNameText.text = statName;
    }


    void Start()
    {
        UpdateStatValueUI();
    }

    //���²ۿ� ��ֵ 
    public void UpdateStatValueUI()
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        if( playerStat != null )
        {
            statValueText.text = playerStat.GetStat(statType).GetValue().ToString();
        }
    }
}
