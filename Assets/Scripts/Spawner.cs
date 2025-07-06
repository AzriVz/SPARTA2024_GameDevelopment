using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{    
    [SerializeField] private GameObject[] items;
    [SerializeField] private float minX = -8f , maxX = 8f;
    [SerializeField] private float minTime = 1.5f, maxTime = 2.5f;
    [SerializeField] private int stepAmount = 5;
    [SerializeField] private float stepSize;
    [SerializeField] private int waktu_awal_spawn;

    // Start is called before the first frame update
    void Start()
    {
        stepSize = (maxX - minX) / stepAmount;
        StartCoroutine(SpawnItems(waktu_awal_spawn));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnItems(float time)
    {
        yield return new WaitForSeconds(time);
        
        Vector3 temp = new Vector3(
            minX + UnityEngine.Random.Range(0, stepAmount + 1) * stepSize, 
            -0.05f,                                          
            transform.position.z                                           
        );
        
        Instantiate(
            items[UnityEngine.Random.Range(0, items.Length)],
            temp,
            Quaternion.identity
        );

        StartCoroutine(SpawnItems(UnityEngine.Random.Range(minTime, maxTime)));
    }
}
