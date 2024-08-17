using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnInZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score;

    private void Awake()
    {
        score = 0;
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }
}
