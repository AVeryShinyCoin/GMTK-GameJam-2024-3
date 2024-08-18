using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMain : MonoBehaviour
{
    //[SerializeField] float rotSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] float acceleration;
    public float TractorBeamPower;
    public float RescaleBeamPower;


    [Space(20)]
    [SerializeField] InputAction movementControlsKeyboard;
    [SerializeField] InputAction scaleChanger;

    [SerializeField] Transform laserBeam;
    [SerializeField] SpriteRenderer laserBeamSr;
    [SerializeField] Sprite sizeIncreaseBeam;
    [SerializeField] Sprite sizeDecreaseBeam;
    [SerializeField] Sprite tractorBeam;
    bool shootingLaser;

    Rigidbody2D rb;
    Transform currentTarget;
    Camera cam;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject explosionPrefab;

    Vector3 cameraVelocity = Vector3.zero;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        laserBeam.SetParent(null);
        laserBeam.gameObject.SetActive(false);
    }

    private void Start()
    {
        SoundManager.Instance.PlayMusic("bgmMusic");
    }

    private void Update()
    {
        if (PauseMenu.Instance.GamePaused) return;

        if (scaleChanger.ReadValue<float>() == 0 && !Input.GetMouseButton(0))
        {
            currentTarget = null;
            StopLaserBeam();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TractorBeam();
            laserBeamSr.sprite = tractorBeam;
            if (!shootingLaser) StartLaserBeam();
        }

        if (scaleChanger.ReadValue<float>() != 0)
        {
            SizeChangeBeam(scaleChanger.ReadValue<float>());
            if (scaleChanger.ReadValue<float>() < 0)
            {
                laserBeamSr.sprite = sizeDecreaseBeam;
            }
            else
            {
                laserBeamSr.sprite = sizeIncreaseBeam;
            }
            if (!shootingLaser) StartLaserBeam();
        }
    }

    void SizeChangeBeam(float value)
    {
        RaycastHit2D[] raycastHits = RaycastEnemy();
        if (raycastHits.Length == 0) return;

        if (raycastHits[0].transform.TryGetComponent(out EnemyObject enemy))
        {
            enemy.ChangeSize(value * RescaleBeamPower);
            currentTarget = raycastHits[0].transform;
        }
    }
    void TractorBeam()
    {
        RaycastHit2D[] raycastHits = RaycastEnemy();
        if (raycastHits.Length == 0) return;

        if (raycastHits[0].transform.TryGetComponent(out EnemyObject enemy))
        {
            enemy.GetPulled(transform.position, TractorBeamPower);
            currentTarget = raycastHits[0].transform;
        }
    }

    private void FixedUpdate()
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

        if (movementControlsKeyboard.ReadValue<Vector2>() != Vector2.zero)
        {
            rb.AddForce(movementControlsKeyboard.ReadValue<Vector2>().normalized * acceleration);
        }

        MoveCamera();
        DisplayLaserBeam();
    }

    void MoveCamera()
    {
        Transform target = transform;
        float dampTime = 0.15f;

        Vector3 point = cam.WorldToViewportPoint(target.position);
        Vector3 delta = target.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = cam.transform.position + delta;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, destination, ref cameraVelocity, dampTime);
    }


    void StartLaserBeam()
    {
        shootingLaser = true;
        DisplayLaserBeam();
    }

    void StopLaserBeam()
    {
        laserBeam.gameObject.SetActive(false);
        shootingLaser = false;
    }

    void DisplayLaserBeam()
    {
        if (!shootingLaser) return;
       
        Vector2 start = transform.position;
        Vector2 end;
        float length = 100;
        if (currentTarget != null)
        {
            end = currentTarget.position;
            length = Vector2.Distance(start, end);
        }
        else
        {
            end = start + (Vector2)transform.right * 100f;
        }

        laserBeam.position = (start + end) / 2;
        laserBeam.rotation = transform.rotation;
        laserBeam.localScale = new Vector3(length, 1.75f, 1.75f);

        laserBeam.gameObject.SetActive(true);
    }

    RaycastHit2D[] RaycastEnemy()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;
        return Physics2D.CircleCastAll(origin, 0.5f, direction, 1000, enemyMask);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out EnemyObject enemy))
        {
            GameOver();
        }
    }
    public void GameOver()
    {
        GameObject gob = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound("Explosion");
        Destroy(gameObject);
        FindAnyObjectByType<GrinderZone>().gameOver = true;
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
