using UnityEngine;


[CreateAssetMenu(fileName = "Ice And Fire effect", menuName = "Data/Item effect/Ice And Fire")]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFireprefab;
    [SerializeField] private float xVelocity = 10f;


    public override void ExecutEffect(Transform _respawnPosition)
    {
        Player player = PlayerManger.instance.player;

        bool thirAttack = player.GetComponent<Player>().primaryAttack.comboCounter == 2;

        if (thirAttack)
        {
             GameObject newIceAndFire = Instantiate(iceAndFireprefab, _respawnPosition.position, player.transform.rotation);

            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
        }
    }
}
