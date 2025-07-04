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

    public void StartFall(float velocity)
    {
      isFalling = true;
      this.velocity = velocity;
    }

    public void StopFall()
    {
      isFalling = false;
    }

    public void Unload()
    {
      
    }
  }
}