using UnityEngine;

public class BlackHole_Skill : Skill
{
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneCoolDown;
    [SerializeField] private float blackHoleDuration;//存在时间

    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    BlackHole_Skill_Controllero currentBlackHole;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);

        currentBlackHole = newBlackHole.GetComponent<BlackHole_Skill_Controllero>();

        currentBlackHole.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneCoolDown, blackHoleDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleteed()
    {
        //需要提前利用它，所以用一个这个来防止报错
        if(!currentBlackHole)
            return false;

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }

    //把随机敌人半径改为黑洞半径的一半
    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
