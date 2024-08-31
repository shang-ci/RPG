using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    //执行物品的效果 
    public virtual void ExecutEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed");
    }
}
