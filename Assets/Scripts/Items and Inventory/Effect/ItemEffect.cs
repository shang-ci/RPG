using UnityEngine;

public class ItemEffect : ScriptableObject
{
    //ִ����Ʒ��Ч�� 
    public virtual void ExecutEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed");
    }
}
