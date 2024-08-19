using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    private void Start()
    {
        FindAnyObjectByType<EnemySpawner>().SpawnZones.Add(GetComponent<Collider2D>());
    }
}
