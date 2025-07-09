using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnableObject
    {
        public GameObject prefab;
        [Range(0f, 1f)]
        public float spawnChance;
        public bool onlySpawnOnce;
    }

    [Header("Spawn Configuration")]
    public SpawnableObject[] objects;
    public float minSpawnRate = 1f;
    public float maxSpawnRate = 2f;
    public float initialDelay = 8f; //delay 8 detik

    private bool[] hasSpawnedOnce;

    private void OnEnable()
    {
        hasSpawnedOnce = new bool[objects.Length];
        Invoke(nameof(StartSpawning), initialDelay);
    }

    private void OnDisable()
    {
        
        CancelInvoke();
    }

    private void StartSpawning()
    {
        Spawn(); 
    }

    private void Spawn()
    {
        float spawnChance = Random.value;

        for (int i = 0; i < objects.Length; i++)
        {
            var obj = objects[i];

            if (obj.onlySpawnOnce && hasSpawnedOnce[i])
                continue;

            if (spawnChance < obj.spawnChance)
            {
                GameObject obstacle = Instantiate(obj.prefab);
                obstacle.transform.position += transform.position;
                break;
            }

            spawnChance -= obj.spawnChance;
        }

        
        Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
    }
}
