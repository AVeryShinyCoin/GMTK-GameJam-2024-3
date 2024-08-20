using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject victoryText;
    [SerializeField] GameObject failureText;

    [SerializeField] TextMeshProUGUI scoreText;


    public void DisplayVictory(int score, int targetScore)
    {
        WinCondition winCondition = FindAnyObjectByType<WinCondition>();

        if (winCondition == null || !winCondition.InfiniteMode)
        {
            scoreText.text = score + " / " + targetScore + "<br>";
            scoreText.color = Color.green;

            failureText.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {
            string time = "";

            float timeRemaining = winCondition.TotalTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);

            string minutesZero = "";
            if (minutes < 10) minutesZero = "0";
            string secondsZero = "";
            if (seconds < 10) secondsZero = "0";

            time = minutesZero + minutes + ":" + secondsZero + seconds;

            scoreText.text = score + "<br>" + winCondition.cyclesComplete + "<br>" + time;
            scoreText.color = Color.green;

            gameObject.SetActive(true);
        }
    }
    
    public void DisplayFailure(int score, int targetScore)
    {
        scoreText.text = score + " / " + targetScore + "<br>";
        scoreText.color = Color.red;

        victoryText.SetActive(false);
        gameObject.SetActive(true);
    }
}
