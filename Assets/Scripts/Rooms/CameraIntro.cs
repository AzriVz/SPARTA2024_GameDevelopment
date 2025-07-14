using UnityEngine;
public class CameraIntro : MonoBehaviour
{
    [SerializeField] Transform waypoint1;
    [SerializeField] Transform waypoint2;
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject introCanvas;
    [SerializeField] GameObject questCanvas;
    [SerializeField] float lerpDuration = 3f;
    float time;

    void Start()
    {
        if (MasterGameManager.Instance.gameStarted)
        {
            if (playerObj != null)
            {
                transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, transform.position.z);
                playerObj.SetActive(true);
                introCanvas.SetActive(false);
                questCanvas.SetActive(true);
            }
            enabled = false;
            return;
        }

        if (waypoint1 == null || waypoint2 == null) return;
        transform.position = waypoint1.position;
        time = 0f;

        if (playerObj != null)
            playerObj.SetActive(false);
    }

    void Update()
    {
        var gm = MasterGameManager.Instance;
        if (!gm.gameStarted)
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
                if (playerObj != null)
                    playerObj.SetActive(true);
                introCanvas.SetActive(false);
                questCanvas.SetActive(true);
                enabled = false;
            }
        }
    }
}
