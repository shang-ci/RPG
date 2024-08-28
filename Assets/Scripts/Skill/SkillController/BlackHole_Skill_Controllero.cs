using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controllero : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;//�����ٶ�
    private float blackHoleTimer;//����ʱ��

    private bool canShrink;
    private bool canGrow = true;
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisapear = true;

    private int amountOfAttacks = 4;//��¡������
    private float cloneAttackCoolDown = .3f;//��¡�˹������
    private float cloneAttackTimer;

    private  List<Transform> tragets = new List<Transform>();//��úڶ���ĵ���λ��
    private  List<GameObject> createHotKey = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    //�������Ϣ����skillManger���棬����duogejineng�໥Ӱ��
    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCoolDown, float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCoolDown = _cloneAttackCoolDown;

        blackHoleTimer = _blackHoleDuration;

        //�ͷ�ˮ����������ʧ����¡���������ʧ
        if (SkillManager.instance.clone.crystalInsteadOfClone)
            playerCanDisapear = false;

    }

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;
 
        //
        if(blackHoleTimer < 0)
        {
            //ֻ��ִ��һ��
            blackHoleTimer = Mathf.Infinity;

            if(tragets.Count > 0) 
                ReleaseCloneAttack();
            else
                FinishBlackHoleAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (tragets.Count <= 0)
            return;

        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKeys = false;

        if (playerCanDisapear)
        {
            playerCanDisapear = false;
            //��¡����ʱ�����������ʧ
            PlayerManger.instance.player.fx. MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCoolDown;

            int randomIndex = Random.Range(0, tragets.Count);

            //ƫ����
            float xOffset;//��¡������˼�ļ��
            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;


            if (SkillManager.instance.clone.crystalInsteadOfClone)
            {
                SkillManager.instance.crystal.CreateCrystal();
                SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
            }
            else
            {
                //��¡����ѡ������ĵ��˹������������ɿ�¡���λ�ò���Ӱ�죬���乥���������
                SkillManager.instance.clone.CreateClone(tragets[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                //���ڶ��������������ﵽ����ʱ����һ����ε��䣬����¡�����ڹ���
                Invoke("FinishBlackHoleAbility", .8f);
            }
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;

        //�ı䡢�Ż������ڶ������ķ�ʽ
        //PlayerManger.instance.player.ExitBlackHoleAbility();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            CreatHotKey(collision);
    }

    //���ܽ����������ڶ��������ֹ
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }

    private void CreatHotKey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;

        if(!canCreateHotKeys)
            return;

        collision.GetComponent<Enemy>().FreezeTime(true);//��ס����

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createHotKey.Add(newHotKey);

        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        BlackHole_HotKey_Controller newHotKeyController = newHotKey.GetComponent<BlackHole_HotKey_Controller>();

        newHotKeyController.SetHotKey(chooseKey, collision.transform, this);
    }

    private void DestroyHotKeys()
    {
        if(createHotKey.Count <= 0)
            return;

        for(int i = 0; i < createHotKey.Count; i++)
        {
            Destroy(createHotKey[i]);
        }
    }

    //��list���ҵĿ���֮��
    public void AddEnemyToList(Transform _enemyTransform) => tragets.Add(_enemyTransform);
}
