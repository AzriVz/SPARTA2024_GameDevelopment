using UnityEngine;
public class CameraIntro : MonoBehaviour
{
    [SerializeField] Transform waypoint1;
    [SerializeField] Transform waypoint2;
    [SerializeField] GameObject playerToActivate;
    [SerializeField] GameObject introCanvas; 
    [SerializeField] float lerpDuration = 3f;
    float time;

    void Start()
    {
        if (MasterGameManager.Instance.IsGameStarted)
        {
            if (playerToActivate != null)
                playerToActivate.SetActive(true);
            enabled = false;
            return;
        }

        if (waypoint1 == null || waypoint2 == null) return;
        transform.position = waypoint1.position;
        time = 0f;

        if (playerToActivate != null)
            playerToActivate.SetActive(false);
    }

    void Update()
    {
        var gm = MasterGameManager.Instance;
        if (!gm.IsGameStarted)
        {
            time += Time.deltaTime / lerpDuration;
            transform.position = Vector3.Lerp(waypoint1.position, waypoint2.position, time);
            if (time >= 1f)
            {
                time = 0f;
                transform.position = waypoint1.position;
            }
            if (gm.IntroPrompt && Input.anyKeyDown)
            {
                gm.StartGame();
                if (playerToActivate != null)
                    playerToActivate.SetActive(true);
                introCanvas.SetActive(false);
                enabled = false;
            }
        }
    }
}
