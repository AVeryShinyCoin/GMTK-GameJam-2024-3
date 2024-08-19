using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] GameObject victoryText;
    [SerializeField] GameObject failureText;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI personalBestText;


    public void DisplayVictory(int score, int targetScore, int bestScore)
    {
        scoreText.text = score + " / " + targetScore;
        scoreText.color = Color.green;
        personalBestText.text = bestScore.ToString();

        failureText.SetActive(false);
        gameObject.SetActive(true);
    }
    
    public void DisplayFailure(int score, int targetScore, int bestScore)
    {
        scoreText.text = score + " / " + targetScore;
        scoreText.color = Color.red;
        personalBestText.text = bestScore.ToString();

        victoryText.SetActive(false);
        gameObject.SetActive(true);
    }
}
