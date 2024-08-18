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
        pattern.ReleasePattern(transform.rotation.eulerAngles.z, GetComponent<Rigidbody2D>().velocity * 0.80f);

        Destroy(gameObject);
    }
}
