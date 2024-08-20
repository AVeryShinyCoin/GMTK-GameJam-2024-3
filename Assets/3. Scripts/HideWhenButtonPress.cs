using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideWhenButtonPress : MonoBehaviour
{
    [SerializeField] InputAction HideButton;

    private void Update()
    {
        if (HideButton.WasPerformedThisFrame())
        {
            gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        HideButton.Enable();
    }
    private void OnDisable()
    {
        HideButton.Disable();
    }
}
