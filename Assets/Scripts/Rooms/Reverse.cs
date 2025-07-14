using UnityEngine;

public class Reverse : MonoBehaviour
{

    [SerializeField] private float offsetSpeed;

    void Update()
    {
        transform.position = new Vector3(transform.position.x - offsetSpeed, transform.position.y, transform.position.z) * Time.deltaTime;
    }
}
