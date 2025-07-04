using System;
using UnityEngine;

namespace Mechanic.Itsuki
{
  [RequireComponent(typeof(SpriteRenderer))]
  public class FoodInstance : MonoBehaviour
  {
    [SerializeField] private Food food;
    [SerializeField] private bool isFalling = false;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private bool isAttached = false;
    private PlayerFoodGrabber _playerFoodGrabber;
    public void Initialize(Food food)
    {
      this.food = food;
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
      if (isAttached)
      {
        if (!isFalling)
        {
          transform.position = _playerFoodGrabber.globalGrabPosition;
        }else 
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