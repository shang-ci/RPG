using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject charaterUI;

    public UI_ItemToolTip itemToolTip;

    void Start()
    {
        //itemToolTip = GetComponentInChildren<UI_ItemToolTip>();
    }

    void Update()
    {
        
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
}
