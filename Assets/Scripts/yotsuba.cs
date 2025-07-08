using UnityEngine;

public class MoveX : MonoBehaviour
{
    public float speed = 2f;
    private float rightEdge;

    private void Start()
    {
        rightEdge = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 2f;
    }

    private void Update()
    {

        transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        if (transform.position.x > rightEdge)
        {
            Destroy(gameObject);
        }
    }
}