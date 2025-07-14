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

    [SerializeField] private LevelManager.SceneID currentScene, targetScene;
    private PlayerManager _playerManager;
    private PlayerFoodGrabber _playerFoodGrabber;
    public void Start()
    {
      VariableSetter();
      StartStage();
    }

    private void VariableSetter()
    {
      _playerManager = PlayerManager.Instance;
      // _playerManager.player.GetComponent<PlayerFoodGrabber>();
    }

    private void StartStage()
    {
      _playerManager.SpawnPlayer();
    }

    public void Win()
    {
      // Win Screen
      Unload();
      if(_playerManager != null) _playerManager.playerHealth.Damagable = false;
      LevelManager.Instance.ChangeLevel(currentScene, targetScene, true);
    }

    private void Unload()
    {
      // if(!_playerFoodGrabber)
      //   _playerFoodGrabber.Unload();
      // _playerManager.DestroyPlayer();
    }
    public void Lose()
    {
      Unload();
      LevelManager.Instance.ChangeLevel(currentScene, targetScene, false);
      // Do some lose screen
    }
  }
}