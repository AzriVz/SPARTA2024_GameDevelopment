using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Mechanic.Itsuki
{
  public class FoodSpawnManager : MonoBehaviour
  {
    [SerializeField] public List<Food> foods;
    [SerializeField] private GameObject foodPrefab;
    [SerializeField] private bool isSpawning = false;
    [FormerlySerializedAs("height")]
    [Header("Spawn Position")]
    [SerializeField] private float heightSpawn;
    [SerializeField] private float minX, maxX;
    [SerializeField] public float heightDespawn;
    [Header("Spawn Interval")]
    [SerializeField] private float minInterval;
    [SerializeField] private float maxInterval;
    [Header("Spawn Velocity")]
    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;
    private float _totalChance;

    #region Singleton
    public static FoodSpawnManager Instance;
    private void Awake() 
    { 
      if (Instance != null && Instance != this) 
      { 
        Destroy(this); 
      } 
      else 
      { 
        Instance = this; 
      } 
    }
    #endregion
    private void OnEnable()
    {
      foreach (var food in foods)
      {
        _totalChance += food.chance;
      }
    }

    public void StartSpawning()
    {
      isSpawning = true;
      StartCoroutine(SpawnRandomFoodCoroutine());
    }

    private void StopSpawning()
    {
      isSpawning = false;
    }
    
    private void Start()
    {
      StartSpawning();
    }

    private IEnumerator SpawnRandomFoodCoroutine()
    {
      while (isSpawning)
      {
        var interval = Random.Range(minInterval, maxInterval);
        yield return new WaitForSeconds(interval);
        SpawnRandomFood();
      }
    }
    
    public void SpawnRandomFood()
    {
      var rand = Random.Range(0f, _totalChance);
      var chanceSum = 0f;
      Food selectedFood = null;
      foreach (var food in foods)
      {
        chanceSum += food.chance;
        if (rand <= chanceSum)
        {
          selectedFood = food;
        }
      }

      if (selectedFood == null)
      {
        Debug.LogError("No food selected");
        return;
      }
      SpawnFood(selectedFood);
    }

    private void SpawnFood(Food food)
    {
      var foodClone = Instantiate(foodPrefab, GetSpawnPosition(), Quaternion.identity);
      var foodInstance = foodClone.GetComponent<FoodInstance>();
      foodInstance.Initialize(food);
      var velocity = Random.Range(minVelocity, maxVelocity);
      foodInstance.StartFall(velocity);
    }

    private Vector2 GetSpawnPosition()
    {
      var x = Random.Range(minX, maxX);
      return new Vector2(x, heightSpawn);
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.cyan;
      Gizmos.DrawLine(new Vector3(minX, heightSpawn, 0), new Vector3(maxX, heightSpawn, 0));
      Gizmos.color = Color.blue;
      Gizmos.DrawLine(new Vector3(minX, heightDespawn, 0), new Vector3(maxX, heightDespawn, 0));
    }
  }
}