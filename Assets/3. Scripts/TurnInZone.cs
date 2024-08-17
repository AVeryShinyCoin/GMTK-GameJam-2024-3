using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnInZone : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject scoreTextPrefab;
    int score;

    private void Awake()
    {
        score = 0;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score + "$";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyObject enemy))
        {
            float scale = enemy.transform.localScale.x;
            float addedScore = enemy.scoreValue * scale / 4;
            int intScore = (int)addedScore;
            score += (int)addedScore;
            UpdateScore();



            GameObject gob = Instantiate(scoreTextPrefab);
            gob.transform.position = collision.transform.position;
            gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + (int)addedScore + "$";

            Destroy(collision.gameObject);
        }
    }
}
