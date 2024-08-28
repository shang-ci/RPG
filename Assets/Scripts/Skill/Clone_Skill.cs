using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;//��¡ԭ��
    [SerializeField] private float cloneDuration;//��¡����ʱ��

    [SerializeField] private bool canAttack;// �ж��Ƿ���Թ���

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;//����ʱ���ɿ�¡���й���

    [Header("Clone can duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal instead of clone")]
    public bool crystalInsteadOfClone;//�ͷźڶ�ʱ�����������ǿ�¡����ˮ������


    public void CreateClone(Transform _clonePosition, Vector3 _offset)
    {
        if (crystalInsteadOfClone)
        {
            SkillManager.instance.crystal.CreateCrystal();

            return;
        }//�����е����ɿ�¡�ļ��ܶ��������ˮ��


        GameObject newClone = Instantiate(clonePrefab);//�����µĿ�¡//��¡ original ���󲢷��ؿ�¡����

        // ����cloneλ�� / ����ʱ��
        newClone.GetComponent<Clone_Skill_Controller>().SetupClone(_clonePosition, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, chanceToDuplicate);                                                                                            //Controller���ڿ�¡ԭ���ϵģ�������GetComponent                                                                                        
    }

    //�ó���������Ŀ�¡�ڿ�ʼ�ͽ�������һ��
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

    //���������һ����¡���̵��ˡ�useless
    public void CanCreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, 0, 0)));
    }

    //�ӳ�����
    private IEnumerator CreateCloneWithDelay(Transform _enemyTransform, Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_enemyTransform, _offset);
    }
}

