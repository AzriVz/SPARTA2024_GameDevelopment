using System;
using UnityEngine;

namespace Mechanic.Itsuki
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class FoodInstance : MonoBehaviour
  {
    [SerializeField] public Food Food {private set; get;}
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private bool isAttached = false;
    private PlayerFoodGrabber _playerFoodGrabber;
    private FoodSpawnManager _foodSpawnManager;

    public void OnEnable()
    {
      _foodSpawnManager = FoodSpawnManager.Instance;
    }

    public void Initialize(Food food)
    {
      this.Food = food;
      GetComponent<SpriteRenderer>().sprite = food.sprite;
    }

    private void FixedUpdate()
    {
      if (isFalling)
      {
        transform.position += Vector3.down * (velocity * Time.deltaTime);
      }
    }

    private void Update()
    {
      Fall();
      DespawnCheck();
    }

    private void DespawnCheck()
    {
      if ((transform.position.y > _foodSpawnManager.heightDespawn)) return;
      Unload();
      Destroy(gameObject);
    }
    private void Fall()
    {
      if (isAttached)
      {
        if (!isFalling)
        {
          transform.position = _playerFoodGrabber.globalGrabPosition;
        }
        else
          Debug.LogWarning("Cant attach while falling");
      }
    }

    public void StartFall(float velocity)
    {
      isFalling = true;
      this.velocity = velocity;
    }

    public void StopFall()
    {
      isFalling = false;
    }

    public void AttachToPlayer(PlayerFoodGrabber playerFoodGrabber)
    {
      _playerFoodGrabber = playerFoodGrabber;
      isAttached = true;
    }

    public void DetatchFromPlayer()
    {
      _playerFoodGrabber = null;
      isAttached = false;
    }
    
    public void Unload()
    {
      
    }
  }
}