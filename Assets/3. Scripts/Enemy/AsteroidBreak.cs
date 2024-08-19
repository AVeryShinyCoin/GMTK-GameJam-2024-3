using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBreak : MonoBehaviour
{
    public float[] scaleBoundires;
    [SerializeField] GameObject breakPatternSmall;
    [SerializeField] GameObject breakPatternMedium;
    [SerializeField] GameObject breakPatternLarge;
    [SerializeField] GameObject unitSpawned;

    [Space(10)]
    public bool SpawnDiamondsOnDestroy;
    [SerializeField] GameObject diamondPrefab;
    [SerializeField] GameObject diamondAsteroidPrefab;


    Rigidbody2D rb;
    Vector2 lastVelocity;

    [Space(10)]
    [SerializeField] float minimumSpeed;
    [SerializeField] float resistancePower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float scale = transform.localScale.x;
        float minimumForce = (minimumSpeed / (scale * 0.3f));
        float impactForce = collision.relativeVelocity.magnitude * collision.transform.localScale.x * transform.localScale.x;
        float resistance = Mathf.Pow(transform.localScale.x, resistancePower);


        //Debug.Log(gameObject.name + ": ImpactForce " + impactForce + ", minForce: " + minimumForce + ", resistance: " + resistance);

        if (minimumForce > impactForce) return;

        if (impactForce > resistance)
        {
            InitiateBreakPattern();
        }
    }

    private void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }

    private void Update()
    {
        if (transform.localScale.x > scaleBoundires[3])
        {
            InitiateBreakPattern();
        }
    }

    public void InitiateBreakPattern()
    {
        float scale = transform.localScale.x;

        if (scale < scaleBoundires[0]) return;

        AsteroidBreakPattern pattern;

        if (scale >= scaleBoundires[0] && scale < scaleBoundires[1]) pattern = Instantiate(breakPatternSmall).GetComponent<AsteroidBreakPattern>();
        else if (scale >= scaleBoundires[1] && scale < scaleBoundires[2]) pattern = Instantiate(breakPatternMedium).GetComponent<AsteroidBreakPattern>();
        else pattern = Instantiate(breakPatternLarge).GetComponent<AsteroidBreakPattern>();

        pattern.Init(transform.position, transform.localScale.x);

        for (int i = 0; i < pattern.NumberOfBreaks; i++)
        {
            if (SpawnDiamondsOnDestroy)
            {
                if (UnityEngine.Random.Range(0, 8) == 0)
                {
                    pattern.SpawnObjectInPattern(diamondAsteroidPrefab, i);
                }
                else
                {
                    pattern.SpawnObjectInPattern(unitSpawned, i);
                }
            }
            else
            {
                pattern.SpawnObjectInPattern(unitSpawned, i);
            }
        }
        List<GameObject> childAsteroids = pattern.ReleasePattern(transform.rotation.eulerAngles.z, GetComponent<Rigidbody2D>().velocity * 1.2f);

        if (SpawnDiamondsOnDestroy)
        {
            SpawnDiamonds();
        }


        Destroy(gameObject);
    }

    public void DestroyAsteroid()
    {
        if (SpawnDiamondsOnDestroy)
        {
            SpawnDiamonds();
        }
        Destroy(gameObject);
    }

    public void SpawnDiamonds()
    {
        int number = (int)(transform.localScale.x / 2) + 1;
        while (number > 0)
        {
            float spawnScale = transform.localScale.x * 0.85f;
            Vector2 spawnPos = (Vector2)transform.position +
                new Vector2(UnityEngine.Random.Range(-spawnScale, spawnScale), UnityEngine.Random.Range(-spawnScale, spawnScale));
            Quaternion spawnRot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360));
            GameObject gob = Instantiate(diamondPrefab, spawnPos, spawnRot);

            gob.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;


            Vector2 scatterForce = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1)).normalized;
            gob.GetComponent<Rigidbody2D>().AddForce(scatterForce * UnityEngine.Random.Range(100f, 200f));

            number--;
        }
    }
}
