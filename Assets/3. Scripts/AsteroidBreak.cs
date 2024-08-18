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

    Rigidbody2D rb;
    Vector2 lastVelocity;

    [Space(10)]
    [SerializeField] float minimumSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float scale = transform.localScale.x;
        float minimumForce = (minimumSpeed / (scale * 0.3f));
        float impactForce = collision.relativeVelocity.magnitude * collision.transform.localScale.x * transform.localScale.x;
        float resistance = Mathf.Pow(transform.localScale.x, 3);


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
            pattern.SpawnObjectInPattern(unitSpawned, i);
        }
        pattern.ReleasePattern(transform.rotation.eulerAngles.z, GetComponent<Rigidbody2D>().velocity * 1.2f);

        Destroy(gameObject);
    }

    public void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}
