using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [Header("Lifetime Range")]
    [SerializeField] public float minLifetime = 10f;
    [SerializeField] public float maxLifetime = 25f;

    public float actualLifetime;

    void Awake()
    {
        actualLifetime = Random.Range(minLifetime, maxLifetime);
    }

    void Start()
    {
        Destroy(gameObject, actualLifetime);
    }
}
