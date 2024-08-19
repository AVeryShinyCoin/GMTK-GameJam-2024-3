using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{
    [SerializeField] float missionTime;
    float timeRemaining;
    public int TargetScore;

    [SerializeField] TextMeshProUGUI targetText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] Slider slider;

    bool timerEnabled;
    public bool QoutaReached;

    private void Start()
    {
        targetText.text = "QOUTA: $" + TargetScore;
        timerText.text = "TIME: $" + TargetScore;
        timeRemaining = missionTime;
    }

    // *** Timer Related stuff ***

    public void StartTimer()
    {
        timerEnabled = true;
    }

    private void Update()
    {
        ProgressTimer();
    }

    void ProgressTimer()
    {
        if (!timerEnabled) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            string minutesZero = "";
            if (minutes < 10) minutesZero = "0";
            string secondsZero = "";
            if (seconds < 10) secondsZero = "0";

            timerText.text = "TIME: " + minutesZero + minutes + ":" + secondsZero + seconds;
        }
        else
        {
            timerText.text = "TIME: 00:00";
            PauseMenu.Instance.EndGame(TargetScore);
        }
    }

    public void UpdateSlider(int score)
    {
        if (QoutaReached) return;
        float value = (float)score / (float)TargetScore;
        if (value >= 1)
        {
            QoutaReached = true;
            value = 1;
            targetText.color = Color.green;
        }
            
        slider.value = value;
    }
}
