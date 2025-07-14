using UnityEngine;

public class StartSound : MonoBehaviour
{
  public string startMusic;

  private void Start()
  {
    AudioManager.instance.PlayMusic(startMusic);
  }
}