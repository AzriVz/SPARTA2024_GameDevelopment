using System;
using UnityEngine;
using UnityEngine.UI;
using Mechanic.Itsuki;
using Unity.VisualScripting;


public class HealthUI : MonoBehaviour
{
    private Health _playerHealth;
    [SerializeField] private Canvas canvas;
    [SerializeField] private Heart heartPrefab;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        PlayerManager.Instance.OnSpawn += UIInit; 
    }

    private void UIInit()
    {
        PlayerManager.Instance.player.GetComponent<Health>().OnHealthChanged += UpdateHeartsUI;
    }

    private void DestroyHearts()
    {
        // while(canvas.transform.childCount > 0)
        //     Destroy(canvas.transform.GetChild(0).gameObject);
        for (var i = canvas.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    private void GenerateHearts(int health, int maxHealth)
    {
        for (var i = 0; i < health; i++)
        {
            var heartClone = Instantiate(heartPrefab, transform);
            heartClone.Initialize(Heart.HeartType.Normal);
        }
        for (var i = 0; i < maxHealth-health; i++)
        {
            var heartClone = Instantiate(heartPrefab, transform);
            heartClone.Initialize(Heart.HeartType.Broken);
        }
    }
    private void UpdateHeartsUI(int health, int maxHealth)
    {
        Debug.Log("updating hearts UI");
        DestroyHearts();
        GenerateHearts(health, maxHealth);
    }
}
