using UnityEngine;
using UnityEngine.UI;
using Mechanic.Itsuki;


public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    private Health playerHealth;
    void Start()
    {
        playerHealth = Object.FindFirstObjectByType<Health>();
    }

    void Update()
    {
        UpdateHeartsUI();
    }

    void UpdateHeartsUI()
    {
        if (playerHealth == null) return; 
            
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < playerHealth.CurrentHealth;
        }
    }
}
