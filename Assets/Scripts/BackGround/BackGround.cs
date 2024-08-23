using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]  private float parallaxEffect;
    private float xPositions;
    private GameObject cm;
    private float length;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cm = GameObject.Find("Main Camera");

        xPositions = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMove = cm.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cm.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPositions + distanceToMove,transform.position.y);

        if (distanceMove > xPositions + length)
            xPositions += length;
        else if(distanceMove < xPositions - length)
            xPositions -= length;
    }
}
