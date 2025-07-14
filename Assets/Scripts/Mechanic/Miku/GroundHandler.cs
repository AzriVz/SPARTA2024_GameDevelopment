using UnityEngine;
using UnityEngine.Events;

public class GroundHandler : MonoBehaviour
{
    public UnityEvent onMiss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            var platform = collision.gameObject.GetComponent<PlatformMiku>();
            if (!platform.isTouched)
            {
                platform.Fail();
                Debug.Log("oof");
                if (onMiss != null) onMiss.Invoke();
            }
            Destroy(collision.gameObject);
        }
    }
}
