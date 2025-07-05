using UnityEngine;

namespace Mechanic.Itsuki
{
  public class ItsukiMouth : InteractableObject
  {
    [SerializeField] private PlayerFoodGrabber playerFoodGrabber;
    [SerializeField] public int nFoodToWin;
    private int _foodCount = 0;
    public override void Interact()
    {
      Debug.Log("ItsukiMouth Interact");
      if (!playerFoodGrabber.FoodInventory.FoodExists()) return;
      playerFoodGrabber.LetGo();
      _foodCount++;
      if (_foodCount >= nFoodToWin)
      {
        Win();
      }
    }

    private void Win()
    {
      Debug.Log("You win!");
    }
  }
}