using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnWhenObjDies : MonoBehaviour
{
    [SerializeField] GameObject targetObject;

    private void Update()
    {
        if (targetObject == null)
        {
            Destroy(gameObject);
        }
    }

}
