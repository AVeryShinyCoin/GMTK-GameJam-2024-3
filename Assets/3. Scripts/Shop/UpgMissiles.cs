using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgMissiles : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ShopButton>().UpgradeAction = UpgradeAction;
    }

    void UpgradeAction()
    {
        FindAnyObjectByType<PlayerMain>().EnableMissiles = true;
    }
}
