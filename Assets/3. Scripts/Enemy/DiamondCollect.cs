using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlayerMain player))
        {
            GetComponentInParent<Diamond>().Collect();
        }
    }
}
