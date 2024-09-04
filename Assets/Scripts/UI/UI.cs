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
}
