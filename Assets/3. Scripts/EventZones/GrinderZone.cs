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
            AsteroidBreak asteroidBreak = collision.GetComponent<AsteroidBreak>();

            if (collision.transform.localScale.x < asteroidBreak.scaleBoundires[0])
            {
                SoundManager.Instance.PlaySoundRandomPitch("AsteroidBreak", 0.9f, 1.1f);
                float scale = enemy.transform.localScale.x;
                float addedScore = enemy.scoreValue * scale / 4;
                PauseMenu.Instance.AddScore((int)addedScore);
                GameObject gob = Instantiate(scoreTextPrefab);
                gob.transform.position = collision.transform.position;
                gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+$" + (int)addedScore;
                asteroidBreak.DestroyAsteroid();
            }
            else
            {
                enemy.BreakIntoPieces();
                SoundManager.Instance.PlaySoundRandomPitch("AsteroidBreak", 0.7f, 1.0f);
            }
        }

        if (collision.gameObject.TryGetComponent(out Diamond diamond))
        {
            SoundManager.Instance.PlaySoundRandomPitch("AsteroidBreak", 1.5f, 1.6f);
            GameObject gob = Instantiate(scoreTextPrefab);
            gob.transform.position = collision.transform.position;
            gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Gem Lost!";
            Destroy(collision.gameObject);
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
