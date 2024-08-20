using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] int cost;
    [TextArea(15, 20)]
    [SerializeField] string upgradeDescription;
    [SerializeField] ShopMenu shopMenu;
    Button button;
    bool boughtUpgrade;
    public Action UpgradeAction;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void UpdateButton()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        if (boughtUpgrade)
        {
            button.interactable = false;
            return;
        }
        if (UpgradePoints.Instance.PointsAvailable < 1)
        {
            button.interactable = false;
            return;
        }
        button.interactable = true;
    }

    public void ButtonPress()
    {
        if (UpgradePoints.Instance.PointsAvailable < 1) return;
        shopMenu.SpendMoney(cost);
        UpgradeAction();
        boughtUpgrade = true;
        button.interactable = false;
        upgradeDescription = "BOUGHT! <br>" + upgradeDescription;
        shopMenu.ShowDiscription(upgradeDescription);
        SoundManager.Instance.PlaySound("Quota");
    }
    public void MouseEnter()
    {
        shopMenu.ShowDiscription(upgradeDescription);
    }
    
    public void MouseExit()
    {
        shopMenu.ShowDiscription("");
    }
}
