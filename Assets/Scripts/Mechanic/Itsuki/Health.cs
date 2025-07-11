using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Mechanic.Itsuki
{
  public class Health : MonoBehaviour
  {
    [SerializeField] private int currentHealth = 3;
    [SerializeField] private int maxHealth = 3;
    private StageManager _stageManager;
    public event Action<int, int> OnHealthChanged;

    public void Start()
    {
      _stageManager = StageManager.Instance;
    }

    public void Initialize(int health)
    {
      maxHealth = health;
      currentHealth = health;
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void Damage()
    {
      currentHealth--;
      if (currentHealth <= 0)
      {
        _stageManager.Lose();
      }
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void ResetHealth()
    {
      currentHealth = maxHealth;
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
   public int CurrentHealth => currentHealth;
  }
}