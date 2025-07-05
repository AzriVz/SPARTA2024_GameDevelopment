using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Itsuki
{
  public class Health : MonoBehaviour
  {
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private int maxHealth = 3;
    public void Initialize(int health)
    {
      maxHealth = health;
      currentHealth = health;
    }

    public void Damage()
    {
      currentHealth--;
    }

    public void ResetHealth()
    {
      currentHealth = maxHealth;
    }
  }
}