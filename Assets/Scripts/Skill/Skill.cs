using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float coolDown;
    protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManger.instance.player;
    }

    protected virtual  void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if(cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = coolDown;
            return true;
        }

        Debug.Log("Unuse skill");
        return false;
    }

    public virtual void UseSkill()
    {

    }
}
