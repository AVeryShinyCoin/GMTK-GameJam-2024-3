using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgPushwave : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ShopButton>().UpgradeAction = UpgradeAction;
    }

    void UpgradeAction()
    {
        FindAnyObjectByType<PlayerMain>().EnablePushwave = true;
    }
}
