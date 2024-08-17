using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject asteroidObject;
    [SerializeField] float[] asteroidSizeRange;

    [SerializeField] Collider2D[] spawnZones;
    [SerializeField] Collider2D targetZone;


    private void Start()
    {
        InvokeRepeating("SpawnAsteroid", 0f, 2.5f);
    }

    void SpawnAsteroid()
    {
        int rnd = Random.Range(0, spawnZones.Length);
        GameObject gob = Instantiate(asteroidObject, RandomPointInBounds(spawnZones[rnd].bounds), Quaternion.identity);

        float scale = Random.Range(asteroidSizeRange[0], asteroidSizeRange[1]);
        int modScale = Random.Range(0, 10);
        if (modScale == 0)
        {
            scale *= 2.5f;
        }
        else if (modScale == 1)
        {
            scale /= 2.5f;
        }

        gob.transform.localScale = new Vector3(scale, scale, scale);
        gob.GetComponent<Rigidbody2D>().mass = scale;

        Vector2 destination = RandomPointInBounds(targetZone.bounds);
        float speed = Random.Range(5, 15);

        gob.GetComponent<Rigidbody2D>().velocity = (destination - (Vector2)gob.transform.position).normalized * speed;
    }


    Vector2 RandomPointInBounds(Bounds bounds)
    {
        return new Vector2(
        Random.Range(bounds.min.x, bounds.max.x),
        Random.Range(bounds.min.y, bounds.max.y));
    }
}
