using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;//克隆原型
    [SerializeField] private float cloneDuration;//克隆持续时间

    [SerializeField] private bool canAttack;// 判断是否可以攻击

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;//反击时生成克隆进行攻击

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;//释放黑洞时，用来决定是克隆还是水晶攻击


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;
        }//让所有的生成克隆的技能都变成生成水晶


        GameObject newClone = Instantiate(clonePrefab);//创建新的克隆//克隆 original 对象并返回克隆对象

        // 调试clone位置 / 持续时间
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate);                                                                                            //Controller绑在克隆原型上的，所以用GetComponent                                                                                        
    }

    //让冲刺留下来的克隆在开始和结束各有一个
    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    //反击后产生一个克隆被刺敌人—useless
    public void CanCreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, 0, 0)));
    }

    //延迟生成
    private IEnumerator CreateCloneWithDelay(Transform _enemyTransform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_enemyTransform, _offset);
    }
}

