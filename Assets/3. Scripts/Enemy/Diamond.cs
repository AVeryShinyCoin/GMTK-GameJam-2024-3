using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    [SerializeField] GameObject scoreTextPrefab;
    [SerializeField] int ScoreValue;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Collect()
    {
        //SoundManager.Instance.PlaySoundRandomPitch("AsteroidBreak", 0.9f, 1.1f);

        PauseMenu.Instance.AddScore(ScoreValue);
        GameObject gob = Instantiate(scoreTextPrefab);
        gob.transform.position = transform.position;
        gob.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+$" + ScoreValue;
        Destroy(gameObject);
    }

    public void GetPulled(Vector2 destination, float force)
    {
        rb.AddForce((destination - (Vector2)transform.position).normalized * (force * 0.75f) * Time.deltaTime);
    }
}
