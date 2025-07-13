using UnityEngine;

public class PlatformMiku : MonoBehaviour
{
    private Transform ground;
    private float hitTime;

    public void Initialize(float hitTime, Transform ground)
    {
        this.hitTime = hitTime;
        this.ground = ground;
    }

    void Update()
    {
        float timeLeft = hitTime - Time.time;
        if (timeLeft <= 0f)
        {
            transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z);
            enabled = false;
            return;
        }

        float deltaY = ground.position.y - transform.position.y;
        float velocityY = deltaY / timeLeft;
        transform.position += Vector3.up * (velocityY * Time.deltaTime);
    }
}
