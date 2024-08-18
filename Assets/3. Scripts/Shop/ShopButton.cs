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
    ShopMenu shopMenu;
    Button button;
    bool boughtUpgrade;
    public Action UpgradeAction;

    private void Awake()
    {
        shopMenu = GetComponentInParent<ShopMenu>();
        button = GetComponent<Button>();
        GetComponentInChildren<TextMeshProUGUI>().text = "$" + cost + "<br>" + GetComponentInChildren<TextMeshProUGUI>().text;
    }

    private void OnEnable()
    {
        if (boughtUpgrade)
        {
            button.interactable = false;
            return;
        }
        if (!shopMenu.CanBuyAtCost(cost))
        {
            button.interactable = false;
            return;
        }
        button.interactable = true;
    }

    public void ButtonPress()
    {
        if (!shopMenu.CanBuyAtCost(cost)) return;
        shopMenu.SpendMoney(cost);
        UpgradeAction();
        boughtUpgrade = true;
        button.interactable = false;
        upgradeDescription = "BOUGHT! <br>" + upgradeDescription;
        shopMenu.ShowDiscription(upgradeDescription);
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
