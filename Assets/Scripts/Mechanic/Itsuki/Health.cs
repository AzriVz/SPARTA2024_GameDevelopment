using System;
using System.Collections;
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
    private SpriteRenderer _spriteRenderer;

    public void Start()
    {
      _spriteRenderer = GetComponent<SpriteRenderer>();
      _stageManager = StageManager.Instance;
    }

    public void Initialize(int health)
    {
      maxHealth = health;
      currentHealth = health;
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private IEnumerator DamageAnimationCoroutine()
    {
      _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
      yield return new WaitForSeconds(0.1f);
      _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
      yield return new WaitForSeconds(0.1f);
      _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
      yield return new WaitForSeconds(0.1f);
      _spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
      yield return new WaitForSeconds(0.1f);
      _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
    public void Damage()
    {
      if (currentHealth <= 0) return;
      AudioManager.instance.PlaySFX("Damage");
      currentHealth--;
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
      StartCoroutine(DamageAnimationCoroutine());
      
      if (currentHealth <= 0)
      {
        _stageManager.Lose();
      }
    }

    public void ResetHealth()
    {
      currentHealth = maxHealth;
      OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    
   public int CurrentHealth => currentHealth;
  }
}