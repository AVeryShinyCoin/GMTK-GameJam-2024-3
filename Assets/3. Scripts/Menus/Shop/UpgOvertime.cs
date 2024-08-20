using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgOvertime : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ShopButton>().UpgradeAction = UpgradeAction;
    }

    void UpgradeAction()
    {
        FindAnyObjectByType<WinCondition>().QuotaTime = 40;
        FindAnyObjectByType<WinCondition>().TimeRemaining += 10;
    }
}
