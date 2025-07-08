using UnityEngine;
using UnityEngine.Events;

public class GroundHandler : MonoBehaviour
{
    public UnityEvent onMiss;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().color != Color.green)
            {
                if (onMiss != null) onMiss.Invoke();
            }
            Destroy(collision.gameObject);
        }
    }
}
