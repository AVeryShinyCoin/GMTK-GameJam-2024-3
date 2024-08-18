using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] float timer;
    [SerializeField] float pushForce;
    [SerializeField] GameObject explosionPrefab;

    void Start()
    {
        SoundManager.Instance.PlaySound("MissileLaunch");
        GetComponent<Rigidbody2D>().velocity = transform.right * 40;
        Invoke("DestroySelf", timer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out AsteroidBreak asteroid))
        {
            if (collision.transform.localScale.x < 4.5f)
            {
                asteroid.DestroyAsteroid();
            }
            else
            {
                Vector2 force = (asteroid.transform.position - transform.position).normalized * pushForce;
                asteroid.GetComponent<Rigidbody2D>().AddForce(force);
                asteroid.InitiateBreakPattern();
            }

            SoundManager.Instance.PlaySound("Explosion");
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        CancelInvoke();
        GameObject gob = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        gob.transform.localScale = new Vector3(5, 5, 5);
        Destroy(gameObject);
    }
}
