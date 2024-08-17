using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeleteEnemyZone : MonoBehaviour
{
    [SerializeField] bool alwaysDelete;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyObject enemy))
        {
            if (enemy.cantBeDeleted && !alwaysDelete) return;
            Destroy(enemy.gameObject);
        }
    }

}
