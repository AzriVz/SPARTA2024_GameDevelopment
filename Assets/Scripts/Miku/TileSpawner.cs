using System.Collections;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public float[] spawnTimes;
    public GameObject tilePrefab;
    [SerializeField] private Transform[] spawnPoints;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float elapsed = 0f;

        for (int i = 0; i < spawnTimes.Length; i++)
        {
            float wait = spawnTimes[i] - elapsed;
            if (wait > 0f)
                yield return new WaitForSeconds(wait);
            else
                yield return null;

            int idx = Random.Range(0, spawnPoints.Length);
            Instantiate(tilePrefab, spawnPoints[idx].position, Quaternion.identity);

            elapsed = spawnTimes[i];
        }
    }
}
