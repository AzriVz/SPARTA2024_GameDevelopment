using System;
using UnityEngine;

namespace Mechanic.Itsuki
{
  public class StageManager : MonoBehaviour
  {
    #region Singleton
    public static StageManager Instance;
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

    private PlayerManager _playerManager;
    public void Start()
    {
      _playerManager = PlayerManager.Instance;
      StartStage();
    }

    private void StartStage()
    {
      _playerManager.SpawnPlayer();
    }

    public void Win()
    {
      // Win Screen
    }

    public void Lose()
    {
      _playerManager.player.GetComponent<PlayerFoodGrabber>().Unload();
      _playerManager.DestroyPlayer();
      // Do some lose screen
    }
  }
}