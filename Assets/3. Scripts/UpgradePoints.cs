using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePoints : MonoBehaviour
{
    public static UpgradePoints Instance;
    public int PointsAvailable;
    public float CurrentTime;
    int pointIntervall;
    bool[] givenPoint = { false, false, false, false, false };

    [SerializeField] GameObject pointAlert;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentTime = 40;
        pointIntervall = 70;
    }
    private void Update()
    {
        CurrentTime += Time.deltaTime;

        for (int i = 1; i < givenPoint.Length - 1; i++)
        {
            if (!givenPoint[i] && CurrentTime > (float)(pointIntervall * i))
            {
                givenPoint[i] = true;
                GivePoint();
            }
        }
    }

    void GivePoint()
    {
        pointAlert.SetActive(true);
        PointsAvailable++;
    }
}
