using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] InputAction toggleMenu;
    [SerializeField] InputAction cat;
    [SerializeField] GameObject instructionsText;

    private void Update()
    {
        if (toggleMenu.WasPerformedThisFrame())
        {
            instructionsText.SetActive(!instructionsText.activeSelf);
        }

        if (cat.WasPerformedThisFrame())
        {
            Debug.Log("triggered");
            SoundManager.Instance.PlaySound("Cat");
        }
    }

    private void OnEnable()
    {
        toggleMenu.Enable();
        cat.Enable();
    }
    private void OnDisable()
    {
        toggleMenu.Disable();
        cat.Disable();
    }
}
