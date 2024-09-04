using UnityEngine;

public class ItemEffect : ScriptableObject
{
    //执行物品的效果 
    public virtual void ExecutEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed");
    }
}
