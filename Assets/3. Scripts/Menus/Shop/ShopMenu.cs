using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI headerText;

    public void ShowDiscription(string description)
    {
        descriptionText.text = description;
    }

    public bool CanBuyAtCost(int cost)
    {
        return UpgradePoints.Instance.PointsAvailable >= cost;
    }

    public void SpendMoney(int cost)
    {
        UpgradePoints.Instance.PointsAvailable -= cost;
        OnEnable();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ShopButton>().UpdateButton();
        }
    }

    private void OnEnable()
    {
        headerText.text = "Upgrades Available:" + UpgradePoints.Instance.PointsAvailable;
    }
}
