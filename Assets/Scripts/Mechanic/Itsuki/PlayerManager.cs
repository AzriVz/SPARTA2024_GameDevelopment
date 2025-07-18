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
    public Health playerHealth;
    [SerializeField] public TextMeshProUGUI textPrompt, textPrompt2;
    public event Action OnSpawn;


    void Update()
    {
      if (player == null) return;
      if (player.transform.position.y < deathY)
      {
        playerHealth.Damage();
        TeleportPlayerToSpawn();
      }
    }

    protected virtual void TeleportPlayerToSpawn()
    {
      player.transform.position = playerSpawn;
    }

    protected virtual void InstantiatePlayer()
    {
      player = Instantiate(playerPrefab, playerSpawn, Quaternion.identity);
    }
    public void SpawnPlayer()
    {
      if (player == null)
      {
        InstantiatePlayer();
      }
      var interact = player.GetComponent<PlayerInteract2D>();
      if (interact)
        interact.Initialize(textPrompt);
      playerHealth = player.GetComponent<Health>();
      OnSpawn?.Invoke();
      
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

    public virtual void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(playerSpawn, 0.5f);
    }
  }
}