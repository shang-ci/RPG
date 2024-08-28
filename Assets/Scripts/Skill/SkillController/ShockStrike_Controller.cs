using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetsStats;
    [SerializeField] private float speed;
    private int damage;

    private Animator anim;
    private bool triggered;

    public void Setup(int _damage, CharacterStats _targetStats)
    {
        damage = _damage;
        targetsStats = _targetStats;
    }

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        if (!targetsStats)
            return;

        if (triggered)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetsStats.transform.position, speed * Time.deltaTime);
        transform.right = transform.position - targetsStats.transform.position;
        
        //世界空间中变换的红轴
        if (Vector2.Distance(transform.position, targetsStats.transform.position) < .1f)
        {
            anim.transform.localRotation = Quaternion.identity;
            anim.transform.localPosition = new Vector3(0, .5f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            triggered = true;

            Invoke("DamageAmdSelfDestroy", .2f);
            anim.SetTrigger("Hit");

        }
    }

    private void DamageAmdSelfDestroy()
    {
        targetsStats.ApplyShock(true);//被连击到的敌人也进入shock
        Destroy(gameObject, .4f);
        targetsStats.TakeDamage(damage);
    }
}
