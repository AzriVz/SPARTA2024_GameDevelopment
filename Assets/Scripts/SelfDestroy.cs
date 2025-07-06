using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
