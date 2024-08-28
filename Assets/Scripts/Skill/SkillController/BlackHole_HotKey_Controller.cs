using TMPro;
using UnityEngine;

public class BlackHole_HotKey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform myEnemy;
    private BlackHole_Skill_Controllero blackHole;


    private void Update()
    {
        if (Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(myEnemy);

            myText.color= Color.clear;
            sr.color = Color.clear;
        }
    }

    public void SetHotKey(KeyCode myNewHotKey, Transform _myEnemy, BlackHole_Skill_Controllero _blackHole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();

        myEnemy = _myEnemy;
        blackHole = _blackHole;

        myHotKey = myNewHotKey; 
        myText.text = myHotKey.ToString();
    }

}
