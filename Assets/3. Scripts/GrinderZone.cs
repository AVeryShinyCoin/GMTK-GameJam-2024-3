using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrinderZone : MonoBehaviour
{
    [SerializeField] GameObject scoreTextPrefab;

    public bool gameOver;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameOver) return;

        if (collision.gameObject.TryGetComponent(out EnemyObject enemy))
        {
            if (collision.transform.localScale.x < collision.GetComponent<AsteroidBreak>().scaleBoundires[0])
            {

            

            float scale = enemy.transform.localScale.x;
            float addedScore = enemy.scoreValue * scale / 4;
            PauseMenu.Instance.AddScore((int)addedScore);
            GameObject gob = Instantiate(scoreTextPrefab);
            gob.transform.position = collision.transform.position;
            gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+$" + (int)addedScore;
            Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.TryGetComponent(out PlayerMain player))
        {
            float addedScore = 300;
            PauseMenu.Instance.AddScore((int)addedScore);
            GameObject gob = Instantiate(scoreTextPrefab);
            gob.transform.position = collision.transform.position;
            gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+$" + (int)addedScore;

            player.GameOver();
        }
    }
}
