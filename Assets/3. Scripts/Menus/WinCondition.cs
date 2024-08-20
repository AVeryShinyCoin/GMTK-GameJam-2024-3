using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinCondition : MonoBehaviour
{

    [SerializeField] float missionTime;
    public float TimeRemaining;
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
    public float TotalTime;
    public float QuotaTime;

    private void Start()
    {
        targetText.text = "QUOTA: $" + TargetScore;
        timerText.text = "TIME: $" + TargetScore;
        TimeRemaining = missionTime;
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

        TimeRemaining -= Time.deltaTime;
        TotalTime += Time.deltaTime;

        if (TimeRemaining > 0)
        {
            int minutes = Mathf.FloorToInt(TimeRemaining / 60);
            int seconds = Mathf.FloorToInt(TimeRemaining % 60);

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
            
            if (!InfiniteMode) SoundManager.Instance.PlaySound("Quota");
        } 
        slider.value = value;
    }

    void InfiniteModeNextStage()
    {
        if (QuotaReached)
        {
            SoundManager.Instance.PlaySound("Quota");
            TimeRemaining = QuotaTime;
            TargetScore += (int)(originalTarget * currentMultiplier);
            currentMultiplier *= multiplier;
            targetText.text = "QOUTA: $" + TargetScore;
            targetText.color = Color.white;
            cyclesComplete++;
            QuotaReached = false;
            UpdateSlider(PauseMenu.Instance.Score);
        }
        else
        {
            PauseMenu.Instance.EndGame(TargetScore);
        }
    }
}
