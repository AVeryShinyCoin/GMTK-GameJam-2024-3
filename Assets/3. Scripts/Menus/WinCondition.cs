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
    public bool QuotaReached;

    [Space(10)]
    public bool InfiniteMode;
    [SerializeField] float multiplier;
    [SerializeField] float currentMultiplier;
    float originalTarget;
    public int cyclesComplete;

    private void Start()
    {
        targetText.text = "QOUTA: $" + TargetScore;
        timerText.text = "TIME: $" + TargetScore;
        timeRemaining = missionTime;
        currentMultiplier = multiplier;
        originalTarget = TargetScore;
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
            if (!InfiniteMode)
            {
                timerText.text = "TIME: 00:00";
                PauseMenu.Instance.EndGame(TargetScore);
            }
            else
            {
                InfiniteModeNextStage();
            }
        }
    }

    public void UpdateSlider(int score)
    {
        if (QuotaReached) return;
        float value = (float)score / (float)TargetScore;
        if (value >= 1)
        {
            QuotaReached = true;
            value = 1;
            targetText.color = Color.green;
            SoundManager.Instance.PlaySound("Quota");
        } 
        slider.value = value;
    }

    void InfiniteModeNextStage()
    {
        if (PauseMenu.Instance.Score >= TargetScore)
        {
            timeRemaining = 30;
            TargetScore += (int)(originalTarget * currentMultiplier);
            currentMultiplier *= multiplier;
        }
        else
        {
            PauseMenu.Instance.EndGame(TargetScore);
        }
    }
}
