namespace Mechanic.Itsuki
{
  public class FoodInventory
  {
    public FoodInstance currentFood;

    public void SetFood(FoodInstance food)
    {
      currentFood = food;
    }

    public FoodInstance GetFood()
    {
      return currentFood;
    }

    public bool FoodExists()
    {
      return currentFood != null;
    }
  }
}