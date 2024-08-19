using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinOnSpawn : MonoBehaviour
{
    private void Start()
    {
        float rnd = Random.Range(150, 400);
        int mod = Random.Range(0, 2);
        if (mod == 1) rnd *= -1;
        GetComponent<Rigidbody2D>().AddTorque(rnd);
    }
}
