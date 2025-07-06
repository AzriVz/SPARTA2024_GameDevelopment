using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject platform;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private int stepAmount = 16;
    
    [Header("Timing")]
    [SerializeField] private float initialDelay = 1f;
    [SerializeField] private float minInterval = 1.5f, maxInterval = 2.5f;
    [SerializeField] private float warningTime = 0.8f;

    private HashSet<float> occupiedSlots = new HashSet<float>();
    private float stepSize;

    void Start()
    {
        stepSize = (maxX - minX) / stepAmount;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // 1) Find a free X slot
            float? xOpt = GetFreeXPosition();
            if (xOpt == null)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            float x = xOpt.Value;
            Vector3 spawnPos = new Vector3(x, platform.transform.position.y + 5.2f, transform.position.z);
            Vector3 warnSpawnPos = new Vector3(x, platform.transform.position.y + 2.5f, transform.position.z);

            // 2) Mark as occupied
            occupiedSlots.Add(x);

            // 3) Show warning
            GameObject warn = Instantiate(warningPrefab, warnSpawnPos, Quaternion.identity);
            yield return new WaitForSeconds(warningTime);
            Destroy(warn);

            // 4) Spawn obstacle
            GameObject obs = Instantiate(
                firePrefab,
                spawnPos,
                Quaternion.identity
            );
            float obstacleLifetime = firePrefab.GetComponent<SelfDestroy>().actualLifetime;

            Destroy(obs, obstacleLifetime);
            StartCoroutine(FreeSlotAfterDelay(x, obstacleLifetime));

            // 5) Wait before next spawn
            float nextInterval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(nextInterval);
        }
    }

    // Returns a random X that isn’t already occupied
    private float? GetFreeXPosition()
    {
        List<float> possibleX = new List<float>(stepAmount + 1);
        for (int i = 0; i <= stepAmount; i++)
        {
            float x = minX + i * stepSize;
            if (!occupiedSlots.Contains(x))
                possibleX.Add(x);
        }
        if (possibleX.Count == 0) return null;
        return possibleX[Random.Range(0, possibleX.Count)];
    }

    // Frees up that slot after the obstacle’s lifetime
    private IEnumerator FreeSlotAfterDelay(float x, float delay)
    {
        yield return new WaitForSeconds(delay);
        occupiedSlots.Remove(x);
    }

    // Draw all possible spawn slots in the editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (stepAmount > 0)
        {
            float size = 0.2f;
            for (int i = 0; i <= stepAmount; i++)
            {
                float x = minX + i * ((maxX - minX) / stepAmount);
                Gizmos.DrawWireSphere(new Vector3(x, transform.position.y, transform.position.z), size);
            }
        }
    }
}
