using System;
using TMPro;
using UnityEngine;

namespace Mechanic.Itsuki
{
  public class PlayerManager : MonoBehaviour
  {
    #region Singleton
    public static PlayerManager Instance;
    private void Awake() 
    { 
      if (Instance != null && Instance != this) 
      { 
        Destroy(this); 
      } 
      else 
      { 
        Instance = this; 
      } 
    }
    #endregion

    public float deathY;
    public GameObject playerPrefab;
    public GameObject player;
    public Vector2 playerSpawn;
    public int playerMaxHealth;
    private Health playerHealth;
    [SerializeField] private TextMeshProUGUI textPrompt;

    private void Start()
    {
    }

    void Update()
    {
      if (player == null) return;
      if (player.transform.position.y < deathY)
      {
        playerHealth.Damage();
      }
    }

    public void SpawnPlayer()
    {
      if (player != null)
      {
        Debug.Log("Player exists, not spawning");
        return;
      }
      player = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
      player.GetComponent<PlayerInteract2D>().Initialize(textPrompt);
      playerHealth = player.GetComponent<Health>();
      playerHealth.Initialize(playerMaxHealth);
    }

    public void DestroyPlayer()
    {
      Destroy(player);
    }
    private void OnDrawGizmos()
    {
      Gizmos.color = Color.magenta;
      Gizmos.DrawLine(new Vector3(-1000, deathY, 0),new Vector3(1000, deathY, 0));
    }

    private void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(playerSpawn, 0.5f);
    }
  }
}