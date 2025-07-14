using UnityEngine;
using TMPro;
using Mechanic.Itsuki;

public class Countdown : MonoBehaviour
{
    [SerializeField] float countdownTime = 30f;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] private GameObject spawner;
    [SerializeField] bool hasEnded;

    void Update()
    {
        countdownTime -= Time.deltaTime;

        if (countdownTime < 0f)
        {
            countdownTime = 0f;
        }
        else if (countdownTime < 10f)
        {
            countdownText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(countdownTime / 60f);
        int seconds = Mathf.FloorToInt(countdownTime % 60f);
        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (countdownTime == 0f && !hasEnded)
        {
            hasEnded = true;
            countdownText.color = Color.red;
            spawner.SetActive(false);
            StageManager.Instance.Win();
        }
    }
}
