using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelRotation : MonoBehaviour
{
    void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
