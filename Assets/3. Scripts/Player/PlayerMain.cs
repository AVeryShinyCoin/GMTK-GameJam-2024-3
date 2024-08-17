using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    //[SerializeField] float rotSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float tractorBeamPower;
    [SerializeField] float sizeChangePower;

    [Space(20)]
    [SerializeField] InputAction movementControlsKeyboard;
    [SerializeField] InputAction scaleChanger;


    Rigidbody2D rb;
    Transform currentTarget;
    Camera cam;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject testObj;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        Vector2 direction;
        if (currentTarget == null)
        {
            direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }
        else
        {
            direction = currentTarget.position - transform.position;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);


        AdjustCamera();

        if (movementControlsKeyboard.ReadValue<Vector2>() != Vector2.zero)
        {
            rb.AddForce(movementControlsKeyboard.ReadValue<Vector2>().normalized * acceleration);
        }

        if (scaleChanger.ReadValue<float>() == 0
            && !Input.GetMouseButton(0))
        {
            currentTarget = null;
            return;
        }
        
        if (scaleChanger.ReadValue<float>() != 0)
        {
            SizeChangeBeam(scaleChanger.ReadValue<float>());
        }

        if (Input.GetMouseButton(0))
        {
            TractorBeam();
        }
    }

    void AdjustCamera()
    {
        Vector2 cameraPos;
        if (currentTarget == null)
        {
            cameraPos = (cam.ScreenToWorldPoint(Input.mousePosition) + transform.position) / 2;
        }
        else
        {
            cameraPos = (currentTarget.position + transform.position) / 2;
        }

        Vector3 newPos = Vector3.Slerp(cam.transform.position, cameraPos, cameraSpeed * Time.deltaTime);
        testObj.transform.position = newPos;
        newPos.z = -10;
        cam.transform.position = newPos;
        
    }


    void SizeChangeBeam(float value)
    {
        RaycastHit2D[] raycastHits = RaycastEnemy();
        if (raycastHits.Length == 0) return;


        if (raycastHits[0].transform.TryGetComponent(out EnemyObject enemy))
        {
            enemy.ChangeSize(value * sizeChangePower);
            currentTarget = raycastHits[0].transform;
        }
    }

    void TractorBeam()
    {
        RaycastHit2D[] raycastHits = RaycastEnemy();
        if (raycastHits.Length == 0) return;

        if (raycastHits[0].transform.TryGetComponent(out EnemyObject enemy))
        {
            enemy.GetPulled(transform.position, tractorBeamPower);
            currentTarget = raycastHits[0].transform;
        }
    }

    RaycastHit2D[] RaycastEnemy()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;
        return Physics2D.CircleCastAll(origin, 0.5f, direction, 1000, enemyMask);
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
