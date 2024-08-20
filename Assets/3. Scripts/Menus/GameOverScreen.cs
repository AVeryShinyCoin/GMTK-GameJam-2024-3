using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject victoryText;
    [SerializeField] GameObject failureText;

    [SerializeField] TextMeshProUGUI scoreText;


    public void DisplayVictory(int score, int targetScore, int bestScore)
    {
        if (!FindAnyObjectByType<WinCondition>().InfiniteMode)
        {
            scoreText.text = score + " / " + targetScore + "<br>" + bestScore;
            scoreText.color = Color.green;

            failureText.SetActive(false);
            gameObject.SetActive(true);
        }
        else
        {

        }
    }
    
    public void DisplayFailure(int score, int targetScore, int bestScore)
    {
        scoreText.text = score + " / " + targetScore + "<br>" + bestScore;
        scoreText.color = Color.red;

        victoryText.SetActive(false);
        gameObject.SetActive(true);
    }
}
