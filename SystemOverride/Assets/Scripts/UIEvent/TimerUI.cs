using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    public float timeRemaining = 100f;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
            timeToDisplay = 0;

        //int minutes = Mathf.FloorToInt(timeToDisplay / 100);
        int seconds = Mathf.FloorToInt(timeToDisplay % 100);

        timerText.text = $"<[{seconds:000}]>";
    }
}
