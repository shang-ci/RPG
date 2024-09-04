using TMPro;
using UnityEngine;


//用于更新UI部分中character里面的 stat槽口 
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

    //更新槽口 的值 
    public void UpdateStatValueUI()
    {
        PlayerStat playerStat = PlayerManger.instance.player.GetComponent<PlayerStat>();

        if( playerStat != null )
        {
            statValueText.text = playerStat.GetStat(statType).GetValue().ToString();
        }
    }
}
