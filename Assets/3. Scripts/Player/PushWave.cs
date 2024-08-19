using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushWave : MonoBehaviour
{
    List<Rigidbody2D> pushedObjects = new();
    [SerializeField] float pushPower;
    Vector2 force;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyObject enemy))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (!pushedObjects.Contains(rb))
            {
                pushedObjects.Add(rb);
                rb.AddForce(force);
            }
        }
    }

    public void Init(GameObject ship)
    {
        force = ((Vector2)transform.position - (Vector2)ship.transform.position).normalized  * pushPower;
        ship.GetComponent<Rigidbody2D>().AddForce(-force * 0.1f);
    }

    private void FixedUpdate()
    {
        foreach (Rigidbody2D rb in pushedObjects)
        {
            
        }
    }

    public void DestroyWave()
    {
        Destroy(transform.parent.gameObject);
    }
}
