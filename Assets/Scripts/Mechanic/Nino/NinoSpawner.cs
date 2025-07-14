using System.Collections;
using Mechanic.Itsuki;
using UnityEngine;

public class NinoSpawner : MonoBehaviour
{
    [Header("What to Spawn")]
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private GameObject projectile;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Timing")]
    [SerializeField] private float initialDelay = 8f; 
    [SerializeField] private float minInterval = 1f;
    [SerializeField] private float maxInterval = 3f;

    [Header("Speed Tweak")]
    [SerializeField] private float velocity = 3f;

    [Header("Duration Tweak")]
    [SerializeField] private float lifetime = 20f;

    private void Start()
    {
        AudioManager.instance.PlayMusic("Nino");
        // collect children as spawn points if none assigned
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            spawnPoints = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
                spawnPoints[i] = transform.GetChild(i);
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        if (initialDelay > 0f)
            yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            int idx = Random.Range(0, spawnPoints.Length);
            Transform point = spawnPoints[idx];

            Instantiate(prefabToSpawn, point.position, point.rotation);

            var tAttack = Instantiate(projectile, point.position, point.rotation);
            tAttack
              .GetComponent<TsundereAttack>()
              .Initialize(projectile.GetComponent<SpriteRenderer>().sprite, lifetime);

            Vector3 direction = (idx == 0) ? Vector3.right : Vector3.left;
            tAttack.GetComponent<TsundereAttack>().Launch(direction, velocity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var p in spawnPoints)
                if (p != null)
                    Gizmos.DrawWireSphere(p.position, 0.2f);
        }
    }
}

