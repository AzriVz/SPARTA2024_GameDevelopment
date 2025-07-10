using UnityEngine;
using TMPro;

public class Countdown : MonoBehaviour
{
    [SerializeField] float countdownTime = 30f;
    [SerializeField] TextMeshProUGUI countdownText;

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

        if (countdownTime == 0f)
        {
            countdownText.color = Color.red;
            //Win
        }
    }
}
