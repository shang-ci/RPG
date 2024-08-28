using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // 力量 增伤1点 爆伤增加 1% 物抗
    public Stat agility;// 敏捷 闪避 1% 闪避几率增加 1%
    public Stat intelligence;// 1 点 魔法伤害 1点魔抗 
    public Stat vitality;//加血的

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;      // 暴击率
    public Stat critPower;       //150% 爆伤

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;//闪避值
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;  // 持续烧伤
    public bool isChilded;  // 削弱护甲 20%
    public bool isShocked;  // 降低敌人命中率

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public System.Action onHealthChanged;//受伤时才更新
    protected bool isDead;

    [SerializeField] public int currentHealth;

    protected virtual  void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    private void Update()
    {
        //所有的状态都设置上默认持续时间，持续过了就结束状态
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;

        if (ignitedTimer < 0)
            isIgnited = false;
        if (chilledTimer < 0)
            isChilded = false;
        if (shockedTimer < 0)
            isShocked = false;

        //防止被点燃后，出现多段伤害后被击飞上天
        if (isIgnited)
           ApplyIgnitedDamage();
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //闪避设置
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        //暴击设置
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        //防御设置
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void DoMagicaDamage(CharacterStats _targetStats)//法伤计算和造成元素效果调用的地方
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totleMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totleMagicalDamage = CheckTargetResistance(_targetStats, totleMagicalDamage);

        _targetStats.TakeDamage(totleMagicalDamage);

        //防止循环在所有元素伤害为0时出现死循环
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;


        //为了防止出现元素伤害一致而导致无法触发元素效果
        //循环判断触发某个元素效果&&让元素效果取决与伤害
        AttemptyToApplyAilement(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void ApplyIgnitedDamage()
    {
        if (ignitedDamageTimer < 0)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();
            ignitedDamageTimer = igniteDamageCooldown;
        }
    }


    private int CheckTargetResistance(CharacterStats _targetStats, int totleMagicalDamage)//法抗计算
    {
        totleMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totleMagicalDamage = Mathf.Clamp(totleMagicalDamage, 0, int.MaxValue);
        return totleMagicalDamage;
    }

    //造成魔法效果&&伤害
    private void AttemptyToApplyAilement(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _lightingDamage && _iceDamage > _fireDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .25f)
            {
                canApplyIgnite = true;
                Debug.Log("Ignited");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .35f)
            {
                canApplyChill = true;
                Debug.Log("Chilled");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            if (Random.value < .55f)
            {
                canApplyShock = true;
                Debug.Log("Shocked");
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        //给燃烧伤害赋值
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }


    //判断异常状态
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilded && !isShocked;
        bool canApplyChill = !isIgnited && !isChilded && !isShocked;
        bool canApplyShock = !isIgnited && !isChilded;

        //使当isShock为真时Shock里的函数仍然可以用

        if (_ignite && canApplyIgnite)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentsDuration;

            fx.IgniteFxFor(ailmentsDuration);
        }
        if (_chill && canApplyChill)
        {
            isChilded = _chill;
            chilledTimer = ailmentsDuration;

            float slowPercentage = .2f;

            GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuration);

            fx.ChillFxFor(ailmentsDuration);
        }
        if (_shock && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);
            }
            else
            {
                //防止出现敌人使玩家进入shock状态后也出现闪电攻击敌人自己
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetWithShockStrike();
            }//isShock为真时反复执行的函数为寻找最近的敌人，创建闪电实例并传入数据

        }
    }

    //触电shock
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }


    //对最近的敌人shock
    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//找到环绕自己的所有碰撞器

        float closestDistance = Mathf.Infinity;//正无穷大的表示形式（只读）
        Transform closestEnemy = null;


        //https://docs.unity3d.com/cn/current/ScriptReference/Mathf.Infinity.html
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)// 防止最近的敌人就是Shock状态敌人自己
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);//拿到与敌人之间的距离
                if (distanceToEnemy < closestDistance)//比较距离，如果离得更近，保存这个敌人的位置,更改最近距离
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//给点燃伤害赋值

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;//雷电伤害赋值


    public virtual  void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpack();
        fx.StartCoroutine("FlashFX");

        if (currentHealth < 0 && !isDead)
            Die();
    }

    public  virtual void Die()
    {
        //防止螺旋上天     —— 有新的解决方法
        //isIgnited = false;
        isDead = true;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilded)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if((Random.Range(0, 100)) < totalCriticalChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCriticalDamage(int _damage)//计算暴击后伤害
    {
        float totleCirticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totleCirticalPower;

        return Mathf.RoundToInt(critDamage);//返回舍入为最近整数的
    }

    //统计生命值
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 10;
    }

    //用来改变当前生命值，不用特效
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
}

