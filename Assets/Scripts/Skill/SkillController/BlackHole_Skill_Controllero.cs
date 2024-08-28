using System.Collections.Generic;
using UnityEngine;

public class BlackHole_Skill_Controllero : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;//收缩速度
    private float blackHoleTimer;//存在时间

    private bool canShrink;
    private bool canGrow = true;
    private bool canCreateHotKeys = true;
    private bool cloneAttackReleased;
    private bool playerCanDisapear = true;

    private int amountOfAttacks = 4;//克隆人数量
    private float cloneAttackCoolDown = .3f;//克隆人攻击间隔
    private float cloneAttackTimer;

    private  List<Transform> tragets = new List<Transform>();//获得黑洞里的敌人位置
    private  List<GameObject> createHotKey = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    //把相关信息传到skillManger里面，方便duogejineng相互影响
    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCoolDown, float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCoolDown = _cloneAttackCoolDown;

        blackHoleTimer = _blackHoleDuration;

        //释放水晶攻击不消失，克隆攻击玩家消失
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
            //只用执行一次
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
            //克隆攻击时，玩家主体消失
            PlayerManger.instance.player.fx. MakeTransprent(true);
        }
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
        {
            cloneAttackTimer = cloneAttackCoolDown;

            int randomIndex = Random.Range(0, tragets.Count);

            //偏移量
            float xOffset;//克隆人与敌人间的间隔
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
                //克隆体有选择最近的敌人攻击，所以生成克隆体的位置不会影响，让其攻击不到玩家
                SkillManager.instance.clone.CreateClone(tragets[randomIndex], new Vector3(xOffset, 0));
            }
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                //当黑洞收缩，即攻击达到次数时，玩家会现形掉落，而克隆人仍在攻击
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

        //改变、优化结束黑洞收缩的方式
        //PlayerManger.instance.player.ExitBlackHoleAbility();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            CreatHotKey(collision);
    }

    //技能结束，收缩黑洞，解除静止
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

        collision.GetComponent<Enemy>().FreezeTime(true);//定住敌人

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

    //让list在我的控制之下
    public void AddEnemyToList(Transform _enemyTransform) => tragets.Add(_enemyTransform);
}
