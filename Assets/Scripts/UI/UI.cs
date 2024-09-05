using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject charaterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;

    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;


    void Start()
    {
        SwitchTo(null);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(charaterUI);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchWithKeyTo(skillTreeUI);

        if(Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if(Input.GetKeyDown (KeyCode.N))
            SwitchWithKeyTo(optionUI);
    }

    //用来选择展示的UI界面 
    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    //根据按键来选择菜单
    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            return;
        }

        SwitchTo(_menu);
    }
}
