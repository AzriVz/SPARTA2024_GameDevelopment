using UnityEngine;

namespace Mechanic.Itsuki
{
  [CreateAssetMenu(fileName = "Food", menuName = "Mechanic/Food")]
  public class Food : ScriptableObject
  {
    public string foodName;
    public Sprite sprite;

    [Tooltip("Relative chance to others, chance/sum(chances)")]
    public float chance;
  }
}