using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [Header("Lifetime Range")]
    [SerializeField] public float minLifetime = 10f;
    [SerializeField] public float maxLifetime = 25f;

    public float actualLifetime;

    void Start()
    {
        actualLifetime = Random.Range(minLifetime, maxLifetime);
        Destroy(gameObject, actualLifetime);
    }
}
