using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] float rotSpeed;
    [SerializeField] InputAction movementControlsKeyboard;
    [SerializeField] InputAction scaleChanger;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);

        if (movementControlsKeyboard.ReadValue<float>() != 0)
        {
            rb.AddRelativeForce(new Vector2(movementControlsKeyboard.ReadValue<float>(), 0));
        }

        if (scaleChanger.WasPressedThisFrame())
        {
            rb.mass += scaleChanger.ReadValue<float>();
            transform.localScale = new Vector3(transform.localScale.x + scaleChanger.ReadValue<float>(),
                transform.localScale.y + scaleChanger.ReadValue<float>(),
                transform.localScale.z + scaleChanger.ReadValue<float>());
        }
    }

    private void OnEnable()
    {
        movementControlsKeyboard.Enable();
        scaleChanger.Enable();
    }
    private void OnDisable()
    {
        movementControlsKeyboard.Disable();
        scaleChanger.Disable();
    }
}
