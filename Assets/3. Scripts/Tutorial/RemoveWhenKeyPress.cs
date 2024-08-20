using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RemoveWhenKeyPress : MonoBehaviour
{
    [SerializeField] InputAction RemoveKey;

    private void Update()
    {
        if (RemoveKey.WasPerformedThisFrame())
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        RemoveKey.Enable();
    }
    private void OnDisable()
    {
        RemoveKey.Disable();
    }
}
