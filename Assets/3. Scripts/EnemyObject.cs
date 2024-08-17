using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{

    Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void GetPulled(Vector2 destination, float force)
    {
        Debug.Log("triggered!");
        rb.AddRelativeForce((destination - (Vector2)transform.position).normalized * force * Time.deltaTime);
    }

    public void ChangeSize(float value)
    {
        float scale = transform.localScale.x;
        if (scale + value * Time.deltaTime > 0.25f)
        {
            scale += value * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            rb.mass = scale;
        }
    }
}
