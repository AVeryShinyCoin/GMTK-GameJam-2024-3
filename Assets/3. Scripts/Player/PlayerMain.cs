using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static EnemySpawner;

public class PlayerMain : MonoBehaviour
{
    public static PlayerMain Instance;

    [SerializeField] float cameraSpeed;
    [SerializeField] float acceleration;
    public float TractorBeamPower;
    public float RescaleBeamPower;
    public bool EnablePushwave;
    public bool EnableMissiles;
    public bool EnableTractorBeam;
    public bool EnableSizeChange;

    [Space(20)]
    [SerializeField] InputAction movementControlsKeyboard;
    [SerializeField] InputAction scaleChanger;
    [SerializeField] InputAction pushWave;
    [SerializeField] InputAction fireMissle;

    [SerializeField] Transform laserBeam;
    [SerializeField] SpriteRenderer laserBeamSr;
    [SerializeField] Sprite sizeIncreaseBeam;
    [SerializeField] Sprite sizeDecreaseBeam;
    [SerializeField] Sprite tractorBeam;
    [SerializeField] BeamStart beamStart;
    bool shootingLaser;

    Rigidbody2D rb;
    Transform currentTarget;
    Camera cam;

    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject pushWavePrefab;
    [SerializeField] GameObject missilePrefab;

    [Space(10)]
    [SerializeField] Slider batterySlider;
    float batteryPowerMax = 100;
    float batteryPower = 100;
    [SerializeField] float batteryRechargeRate;
    [SerializeField] float missileCost;
    [SerializeField] float pushCost;
    bool holdMissile;
    bool holdPush;

    Vector3 cameraVelocity = Vector3.zero;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        //laserBeam.SetParent(null);
        laserBeam.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (PauseMenu.Instance.GamePaused) return;

        RechargeBatteryPower();

        if (pushWave.WasPerformedThisFrame())
        {
            UsePushwave();
        }
        else if (pushWave.IsInProgress())
        {
            if (holdPush)
            {
                UsePushwave();
            }
        }
        else
        {
            holdPush = false;
        }

        if (fireMissle.WasPerformedThisFrame())
        {
            FireMissile();
        }
        else if (fireMissle.IsInProgress())
        {
            if (holdMissile)
            {
                FireMissile();
            }
        }
        else
        {
            holdMissile = false;
        }

        if (scaleChanger.ReadValue<float>() == 0 && !Input.GetMouseButton(0))
        {
            currentTarget = null;
            StopLaserBeam();
            return;
        }

        if (Input.GetMouseButton(0) && EnableTractorBeam)
        {
            TractorBeam();
            laserBeamSr.sprite = tractorBeam;
            beamStart.OrangeBeam();
            UpdateBeamSound();
            if (!shootingLaser) StartLaserBeam();
        }

        if (scaleChanger.ReadValue<float>() != 0 && EnableSizeChange)
        {
            SizeChangeBeam(scaleChanger.ReadValue<float>());
            if (scaleChanger.ReadValue<float>() < 0)
            {
                laserBeamSr.sprite = sizeDecreaseBeam;
                beamStart.PurpleBeam();
            }
            else
            {
                laserBeamSr.sprite = sizeIncreaseBeam;
                beamStart.GreenBeam();
            }
            UpdateBeamSound();
            if (!shootingLaser) StartLaserBeam();
        }
    }

    void SizeChangeBeam(float value)
    {
        currentTarget = RaycastEnemy();
        if (currentTarget == null) return;

        if (currentTarget.TryGetComponent(out EnemyObject enemy))
        {
            enemy.ChangeSize(value * RescaleBeamPower);
        }
    }
    void TractorBeam()
    {
        currentTarget = RaycastEnemy();
        if (currentTarget == null) return;

        if (currentTarget.TryGetComponent(out EnemyObject enemy))
        {
            enemy.GetPulled(transform.position, TractorBeamPower);
        }
        else if (currentTarget.TryGetComponent(out Diamond diamond))
        {
            diamond.GetPulled(transform.position, TractorBeamPower);
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
        if (!shootingLaser)
        {
            shootingLaser = true;
            DisplayLaserBeam();
        }
    }

    void StopLaserBeam()
    {
        if (shootingLaser)
        {
            laserBeam.gameObject.SetActive(false);
            shootingLaser = false;
        }
        beamStart.TurnOffBeam();
        UpdateBeamSound();
    }

    void UpdateBeamSound()
    {
        if (!shootingLaser)
        {
            SoundManager.Instance.StopSound("ShrinkBeamLoop");
            SoundManager.Instance.StopSound("EnlargeBeamLoop");
            SoundManager.Instance.StopSound("TractorBeamLoop");
        }
        else
        {
            float scale = scaleChanger.ReadValue<float>();
            if (scale != 0 && EnableSizeChange)
            {
                if (scale < 0)
                {
                    SoundManager.Instance.PlaySound("ShrinkBeamLoop");
                    SoundManager.Instance.StopSound("EnlargeBeamLoop");
                }
                else
                {
                    SoundManager.Instance.PlaySound("EnlargeBeamLoop");
                    SoundManager.Instance.StopSound("ShrinkBeamLoop");
                }
            }
            else
            {
                SoundManager.Instance.StopSound("ShrinkBeamLoop");
                SoundManager.Instance.StopSound("EnlargeBeamLoop");
            }

            if (Input.GetMouseButton(0))
            {
                SoundManager.Instance.PlaySound("TractorBeamLoop");
            }
            else
            {
                SoundManager.Instance.StopSound("TractorBeamLoop");
            }
        }
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

        //laserBeam.position = (start + end) / 2f;
        laserBeam.rotation = transform.rotation;
        laserBeam.localScale = new Vector3(length, 1.75f, 1.75f);

        laserBeam.gameObject.SetActive(true);


        //Vector2 start = beamStart.transform.position;
        //Vector2 end;
        //float length = 100;
        //if (currentTarget != null)
        //{
        //    end = currentTarget.position;
        //    length = Vector2.Distance(start, end);
        //}
        //else
        //{
        //    end = start + (Vector2)transform.right * 100f;
        //}

        //laserBeam.position = (start + end) / 2f;
        //laserBeam.rotation = transform.rotation;
        //laserBeam.localScale = new Vector3(length, 1.75f, 1.75f);

        //laserBeam.gameObject.SetActive(true);
    }

    Transform RaycastEnemy()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.right;
        RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(origin, 0.5f, direction, 1000, enemyMask);

        if (raycastHits.Length == 0) return null;

        int targetIndex = 0;
        // If multiple hits are in array, check first if it contains current target. If not, find closest target
        if (raycastHits.Length > 1)
        {
            bool containsCurrentTarget = false;
            for (int i = 0; i < raycastHits.Length; i++)
            {
                if (raycastHits[i].collider.transform == currentTarget)
                {
                    targetIndex = i;
                    containsCurrentTarget = true;
                    break;
                }
            }
            if (!containsCurrentTarget)
            {
                Transform closestTarget = raycastHits[0].transform;
                float closestDistance = Vector2.Distance(transform.position, closestTarget.position);

                for (int i = 1; i < raycastHits.Length; i++)
                {
                    float distance = Vector2.Distance(transform.position, raycastHits[i].collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestTarget = raycastHits[i].transform;
                        closestDistance = distance;
                        targetIndex = i;
                    }
                }
            }
        }

        if (raycastHits[targetIndex].transform.TryGetComponent(out EnemyObject enemy))
        {
            return raycastHits[targetIndex].transform;
        }
        else if (raycastHits[targetIndex].transform.TryGetComponent(out Diamond diamond))
        {
            return raycastHits[targetIndex].transform;
        }
        else
        {
            return null;
        }
    }

    void UsePushwave()
    {
        if (!EnablePushwave) return;
        if (batteryPower < pushCost)
        {
            holdPush = true;
            return;
        }

        batteryPower -= pushCost;
        SoundManager.Instance.PlaySound("PushBack");
        GameObject gob = Instantiate(pushWavePrefab);
        gob.transform.position = transform.position + transform.right * 0.5f;
        gob.transform.rotation = transform.rotation;
        gob.SetActive(true);
        gob.GetComponentInChildren<PushWave>().Init(gameObject);
    }

    void FireMissile()
    {
        if (!EnableMissiles) return;
        if (batteryPower < missileCost)
        {
            holdMissile = true;
            return;
        }

        batteryPower -= missileCost;
        GameObject gob = Instantiate(missilePrefab);
        gob.transform.position = transform.position + transform.right * 0.5f;
        gob.transform.rotation = transform.rotation;
        gob.SetActive(true);
    }

    void RechargeBatteryPower()
    {
        if (batteryPower < batteryPowerMax)
        {
            batterySlider.gameObject.SetActive(true);
            batteryPower += batteryRechargeRate * Time.deltaTime;
            batterySlider.value = batteryPower;
        }
        else
        {
            batterySlider.gameObject.SetActive(false);
        }
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
        SoundManager.Instance.StopSound("ShrinkBeamLoop");
        SoundManager.Instance.StopSound("EnlargeBeamLoop");
        SoundManager.Instance.StopSound("TractorBeamLoop");
        GameObject gob = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound("Explosion");
        StopLaserBeam();
        PauseMenu.Instance.InvokeGameOver();
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        movementControlsKeyboard.Enable();
        scaleChanger.Enable();
        pushWave.Enable();
        fireMissle.Enable();
    }
    private void OnDisable()
    {
        movementControlsKeyboard.Disable();
        scaleChanger.Disable();
        pushWave.Disable();
        fireMissle.Disable();
    }
}
