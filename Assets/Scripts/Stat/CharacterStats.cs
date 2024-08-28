using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stat strength; // ���� ����1�� �������� 1% �￹
    public Stat agility;// ���� ���� 1% ���ܼ������� 1%
    public Stat intelligence;// 1 �� ħ���˺� 1��ħ�� 
    public Stat vitality;//��Ѫ��

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;      // ������
    public Stat critPower;       //150% ����

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;//����ֵ
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;  // ��������
    public bool isChilded;  // �������� 20%
    public bool isShocked;  // ���͵���������

    [SerializeField] private float ailmentsDuration = 4;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = .3f;
    private float ignitedDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;


    public System.Action onHealthChanged;//����ʱ�Ÿ���
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
        //���е�״̬��������Ĭ�ϳ���ʱ�䣬�������˾ͽ���״̬
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

        //��ֹ����ȼ�󣬳��ֶ���˺��󱻻�������
        if (isIgnited)
           ApplyIgnitedDamage();
    }


    public virtual void DoDamage(CharacterStats _targetStats)
    {
        //��������
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        //��������
        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);
        }

        //��������
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void DoMagicaDamage(CharacterStats _targetStats)//���˼�������Ԫ��Ч�����õĵط�
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totleMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totleMagicalDamage = CheckTargetResistance(_targetStats, totleMagicalDamage);

        _targetStats.TakeDamage(totleMagicalDamage);

        //��ֹѭ��������Ԫ���˺�Ϊ0ʱ������ѭ��
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;


        //Ϊ�˷�ֹ����Ԫ���˺�һ�¶������޷�����Ԫ��Ч��
        //ѭ���жϴ���ĳ��Ԫ��Ч��&&��Ԫ��Ч��ȡ�����˺�
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


    private int CheckTargetResistance(CharacterStats _targetStats, int totleMagicalDamage)//��������
    {
        totleMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totleMagicalDamage = Mathf.Clamp(totleMagicalDamage, 0, int.MaxValue);
        return totleMagicalDamage;
    }

    //���ħ��Ч��&&�˺�
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

        //��ȼ���˺���ֵ
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
    }


    //�ж��쳣״̬
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApplyIgnite = !isIgnited && !isChilded && !isShocked;
        bool canApplyChill = !isIgnited && !isChilded && !isShocked;
        bool canApplyShock = !isIgnited && !isChilded;

        //ʹ��isShockΪ��ʱShock��ĺ�����Ȼ������

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
                //��ֹ���ֵ���ʹ��ҽ���shock״̬��Ҳ�������繥�������Լ�
                if (GetComponent<Player>() != null)
                    return;
                HitNearestTargetWithShockStrike();
            }//isShockΪ��ʱ����ִ�еĺ���ΪѰ������ĵ��ˣ���������ʵ������������

        }
    }

    //����shock
    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentsDuration;

        fx.ShockFxFor(ailmentsDuration);
    }


    //������ĵ���shock
    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);//�ҵ������Լ���������ײ��

        float closestDistance = Mathf.Infinity;//�������ı�ʾ��ʽ��ֻ����
        Transform closestEnemy = null;


        //https://docs.unity3d.com/cn/current/ScriptReference/Mathf.Infinity.html
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)// ��ֹ����ĵ��˾���Shock״̬�����Լ�
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);//�õ������֮��ľ���
                if (distanceToEnemy < closestDistance)//�ȽϾ��룬�����ø���������������˵�λ��,�����������
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

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;//����ȼ�˺���ֵ

    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;//�׵��˺���ֵ


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
        //��ֹ��������     ���� ���µĽ������
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

    private int CalculateCriticalDamage(int _damage)//���㱩�����˺�
    {
        float totleCirticalPower = (critPower.GetValue() + strength.GetValue()) * .01f;

        float critDamage = _damage * totleCirticalPower;

        return Mathf.RoundToInt(critDamage);//��������Ϊ���������
    }

    //ͳ������ֵ
    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 10;
    }

    //�����ı䵱ǰ����ֵ��������Ч
    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if (onHealthChanged != null)
        {
            onHealthChanged();
        }
    }
}

