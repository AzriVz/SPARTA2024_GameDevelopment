using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Itsuki
{
  public class PlayerFoodGrabber : MonoBehaviour
  {
    private FoodInventory _foodInventory;
    [SerializeField] private Vector2 relativeGrabPosition;
    public Vector2 globalGrabPosition;
    [SerializeField] private float grabRange;

    private void Awake()
    {
      _foodInventory = new FoodInventory();
    }
    private void Update()
    {
      SetGlobalPosition();
      TryGrab();
    }

    private void SetGlobalPosition()
    {
      globalGrabPosition = transform.position + new Vector3(relativeGrabPosition.x, relativeGrabPosition.y, 0);
    }

    private void TryGrab()
    {
      if (_foodInventory.FoodExists()) return;
      var foodInstance = GetClosestFood();
      if (foodInstance == null) return;
      Debug.Log(foodInstance);
      _foodInventory.SetFood(foodInstance);
      Grab();
    }

    private void Grab()
    {
      var foodInstance = _foodInventory.GetFood();
      foodInstance.StopFall();
      foodInstance.AttachToPlayer(this);
    }

    private void LetGo()
    {
      var foodInstance = _foodInventory.GetFood();
      foodInstance.Unload();
      Destroy(foodInstance.gameObject);
      _foodInventory.SetFood(null);
    }

    private FoodInstance GetClosestFood()
    {
      var hits = Physics2D.OverlapCircleAll(globalGrabPosition, grabRange);
      
      Collider2D closest = null;
      var minDist = Mathf.Infinity;

      foreach (var hit in hits)
      {
          if (!hit.CompareTag("Food")) continue;

          var dist = Vector2.Distance(transform.position, hit.transform.position);
          if (!(dist < minDist)) continue;
          closest = hit;
          minDist = dist;
      }

      if (closest == null || closest.GetComponent<FoodInstance>() == null) return null;
      var foodInstance = closest.GetComponent<FoodInstance>();
      return foodInstance;
    }
    
    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawWireCube(globalGrabPosition, Vector3.one);
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, grabRange);
    }
  }
}