using UnityEngine;

public class RoomStartSound : MonoBehaviour
{
    public string startMusic;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (string.IsNullOrEmpty(startMusic)) return;
            Debug.Log($"Play {startMusic}");
            if (AudioManager.instance.MusicPLayed() != startMusic)
            AudioManager.instance.PlayMusic(startMusic);
        }
    }


}