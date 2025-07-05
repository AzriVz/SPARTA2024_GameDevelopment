using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mechanic.Itsuki
{
  public class AttackManager : MonoBehaviour
  {
    [SerializeField] private GameObject attackPrefab;
    [SerializeField] private List<Sprite> tsundereSprites;
    [SerializeField] private bool isSpawning = false;
    [Header("Spawn Interval")]
    [SerializeField] private float minInterval;
    [SerializeField] private float maxInterval;
    [Header("Spawn Velocity")]
    [SerializeField] private float minVelocity;
    [SerializeField] private float maxVelocity;
    public List<SpawnPoint> spawnPoints;
    public enum Direction { Up, Down, Left, Right }
    [Serializable]
    public struct SpawnPoint
    {
      public Transform transform;
      public Direction direction;
      public Vector2 GetPosition() {return transform.position;}
    }

    private void StartSpawning()
    {
      isSpawning = true;
    }

    private void StopSpawning()
    {
      isSpawning = false;
    }
    private void Start()
    {
      StartSpawning();
      StartCoroutine(SpawnCoroutine());
    }
    
    private IEnumerator SpawnCoroutine()
    {
      while (true)
      {
        if (!isSpawning) break;
        
        SpawnAttack();
        
        var interval = Random.Range(minInterval, maxInterval);
        yield return new WaitForSeconds(interval);
      }
    }
    private void SpawnAttack()
    {
      var spawnPoint = GetRandomSpawn();
      var attackClone = Instantiate(attackPrefab,spawnPoint.GetPosition(), Quaternion.identity);
      var attackComponent = attackClone.GetComponent<TsundereAttack>();
      attackComponent.Initialize(GetRandomSprite());
      var velocity = Random.Range(minVelocity, maxVelocity);
      attackComponent.Launch(ParseDirection(spawnPoint.direction),velocity);
    }

    private Vector3 ParseDirection(Direction direction)
    {
      return direction switch
      {
        Direction.Up => Vector3.up,
        Direction.Down => Vector3.down,
        Direction.Left => Vector3.left,
        Direction.Right => Vector3.right,
        _ => Vector3.right
      };
    }
    private SpawnPoint GetRandomSpawn()
    {
      return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }

    private Sprite GetRandomSprite()
    {
      return tsundereSprites[Random.Range(0, tsundereSprites.Count)];
    }

  }

}