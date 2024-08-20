using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField]  private float parallaxEffect;
    private float xPositions;
    private GameObject cm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cm = GameObject.Find("Main Camera");

        xPositions = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToMove = cm.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPositions + distanceToMove,transform.position.y);
    }
}
