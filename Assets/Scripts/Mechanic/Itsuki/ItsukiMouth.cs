using System;
using UnityEngine;

namespace Mechanic.Itsuki
{
  public class ItsukiMouth : InteractableObject
  {
    [SerializeField] private PlayerFoodGrabber playerFoodGrabber;
    [SerializeField] public int nFoodToWin;
    private int _foodCount = 0;
    private StageManager _stageManager;
    
    private void Start()
    {
      _stageManager = StageManager.Instance;
      PlayerManager.Instance.OnSpawn += SetFoodGrabber;
    }

    private void SetFoodGrabber()
    {
      playerFoodGrabber = PlayerManager.Instance.player.GetComponent<PlayerFoodGrabber>();
    }

    public override void Interact()
    {
      if (!playerFoodGrabber) return;
      if (!playerFoodGrabber.FoodInventory.FoodExists()) return;
      playerFoodGrabber.LetGo();
      _foodCount++;
      if (_foodCount >= nFoodToWin)
      {
        _stageManager.Win();
      }
    }

  }
}