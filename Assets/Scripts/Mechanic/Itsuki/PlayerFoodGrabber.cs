using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Itsuki
{
  public class PlayerFoodGrabber : MonoBehaviour
  {
    public FoodInventory FoodInventory {private set; get;}
    [SerializeField] private Vector2 relativeGrabPosition;
    public Vector2 globalGrabPosition;
    [SerializeField] private float grabRange;
    TextMeshProUGUI _textObject;
    private PlayerInteract2D _playerInteract;

    private void Awake()
    {
      FoodInventory = new FoodInventory();
      _playerInteract = GetComponent<PlayerInteract2D>();
    }

    private void Start()
    {
      _textObject = PlayerManager.Instance.textPrompt2;
    }

    private void Update()
    {
      SetGlobalPosition();
      TryGrab();
      Debug.Log(FoodInventory.FoodExists() + " " + (_playerInteract.currentTarget == null));
      if (FoodInventory.FoodExists())
      {
        _textObject.text = "(X) Throw";
      }
      else
      {
        _textObject.text = "";
      }
      if (Input.GetKeyDown(KeyCode.X))
      {
        TryThrow();
      }
    }

    private void TryThrow()
    {
      LetGo();
    }

    private void SetGlobalPosition()
    {
      globalGrabPosition = transform.position + new Vector3(relativeGrabPosition.x, relativeGrabPosition.y, 0);
    }

    private void TryGrab()
    {
      if (FoodInventory.FoodExists()) return;
      var foodInstance = GetClosestFood();
      if (foodInstance == null) return;
      Debug.Log(foodInstance);
      FoodInventory.SetFood(foodInstance);
      Grab();
    }

    private void Grab()
    {
      var foodInstance = FoodInventory.GetFood();
      foodInstance.StopFall();
      foodInstance.AttachToPlayer(this);
      AudioManager.instance.PlaySFX("CatchFood");
    }

    public void LetGo()
    {
      var foodInstance = FoodInventory.GetFood();
      if(foodInstance == null) return;
      foodInstance.Unload();
      Destroy(foodInstance.gameObject);
      FoodInventory.SetFood(null);
    }

    public void Unload()
    {
      LetGo();
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