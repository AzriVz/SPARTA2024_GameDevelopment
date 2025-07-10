using System.Collections;
using Mechanic.Itsuki;
using UnityEngine;

public class Fire : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D other)
    {
      var health = other.GetComponent<Health>();
      if (health == null) return;
      health.Damage();
    }
}
