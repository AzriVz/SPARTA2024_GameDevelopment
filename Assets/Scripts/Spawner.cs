using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{    
    [Header("Prefabs")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject warningPrefab;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -8f, maxX = 8f;
    [SerializeField] private int stepAmount = 2;

    [Header("Timing")]
    [Tooltip("Time before first spawn")]
    [SerializeField] private float initialDelay = 3f;
    [Tooltip("Random interval between spawns")]
    [SerializeField] private float minInterval = 1.5f, maxInterval = 2.5f;
    [Tooltip("How long warning shows before obstacle")]
    [SerializeField] private float warningTime = 0.8f;
    [Tooltip("How long obstacle lives")]
    [SerializeField] private float obstacleLifetime = 5f;

    private float stepSize;

    void Start()
    {
        stepSize = (maxX - minX) / stepAmount;
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        // initial wait
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            // pick random X slot
            float x = minX + UnityEngine.Random.Range(0, stepAmount + 1) * stepSize;
            Vector3 spawnPos = new Vector3(x, -0.1f, transform.position.z);
            Vector3 warnSpawnPos = new Vector3(x, -2.8f, transform.position.z);

            // 1) show warning
            GameObject warn = Instantiate(warningPrefab, warnSpawnPos, Quaternion.identity);

            // 2) wait warningTime
            yield return new WaitForSeconds(warningTime);

            // 3) destroy warning, spawn obstacle
            Destroy(warn);
            var obs = Instantiate(
                obstaclePrefabs[UnityEngine.Random.Range(0, obstaclePrefabs.Length)],
                spawnPos,
                Quaternion.identity
            );
            Destroy(obs, obstacleLifetime);

            // 4) wait next interval
            float nextInterval = UnityEngine.Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(nextInterval);
        }
    }
}
