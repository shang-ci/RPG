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

    //����ѡ��չʾ��UI���� 
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

    //���ݰ�����ѡ��˵�
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
