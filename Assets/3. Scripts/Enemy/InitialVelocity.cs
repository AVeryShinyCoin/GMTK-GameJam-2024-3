using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocity : MonoBehaviour
{
    [SerializeField] Vector2 initialMovement;

    private void Start()
    {
        if (initialMovement != Vector2.zero)
        {
            GetComponent<Rigidbody2D>().velocity = initialMovement;
        }
    }
}
