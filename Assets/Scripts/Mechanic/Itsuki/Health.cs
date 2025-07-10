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

    public void Start()
    {
      _stageManager = StageManager.Instance;
    }

    public void Initialize(int health)
    {
      maxHealth = health;
      currentHealth = health;
    }

    public void Damage()
    {
      Debug.Log("oof");
      currentHealth--;
      if (currentHealth <= 0)
      {
        _stageManager.Lose();
      }
    }

    public void ResetHealth()
    {
      currentHealth = maxHealth;
    }
    
   public int CurrentHealth => currentHealth;
  }
}