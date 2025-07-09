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
    [Tooltip("Minimum seconds between spawns")]
    [SerializeField] private float minInterval = 1f;
    [Tooltip("Maximum seconds between spawns")]
    [SerializeField] private float maxInterval = 3f;

    [Header("Speed Tweak")]
    [SerializeField] private float velocity = 1f;

    private void Start()
    {
        // Grab all children of this GameObject
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
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            int idx = Random.Range(0, spawnPoints.Length);
            Transform point = spawnPoints[idx];

            Instantiate(prefabToSpawn, point.position, point.rotation);
            var tAttack = Instantiate(projectile, point.position, point.rotation);
            Vector3 direction = (idx == 0) ? Vector3.right : Vector3.left;
            // 5) rotate it visually to face movement
            tAttack.transform.right = direction;
            tAttack.GetComponent<TsundereAttack>().Launch(point.position, velocity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var p in spawnPoints)
            {
                if (p != null)
                    Gizmos.DrawWireSphere(p.position, 0.2f);
            }
        }
    }
}
