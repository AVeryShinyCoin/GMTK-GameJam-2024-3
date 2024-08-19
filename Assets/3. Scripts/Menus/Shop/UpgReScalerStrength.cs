using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgReScalerStrength : MonoBehaviour
{
    [SerializeField] float newStrength;

    private void Start()
    {
        GetComponent<ShopButton>().UpgradeAction = UpgradeAction;
    }

    void UpgradeAction()
    {
        FindAnyObjectByType<PlayerMain>().RescaleBeamPower = newStrength;
    }
}
