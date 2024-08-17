using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{

    Rigidbody2D rb;
    public float scoreValue;
    public bool cantBeDeleted = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("BecomeDeletable", 5f);
    }

    public void GetPulled(Vector2 destination, float force)
    {
        rb.AddForce((destination - (Vector2)transform.position).normalized * force * Time.deltaTime);
    }

    public void ChangeSize(float value)
    {
        float scale = transform.localScale.x;
        if (value > 0 || scale + value * Time.deltaTime > 0.25f)
        {
            scale += value * Time.deltaTime;
            transform.localScale = new Vector3(scale, scale, scale);
            rb.mass = scale;
        }
    }

    void BecomeDeletable()
    {
        cantBeDeleted = false;
    }
}
