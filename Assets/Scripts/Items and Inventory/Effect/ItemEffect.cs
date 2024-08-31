using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item effect")]
public class ItemEffect : ScriptableObject
{
    //ִ����Ʒ��Ч�� 
    public virtual void ExecutEffect(Transform _enemyPosition)
    {
        Debug.Log("Effect executed");
    }
}
