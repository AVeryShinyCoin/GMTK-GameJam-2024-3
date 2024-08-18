using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI descriptionText;




    public void ShowDiscription(string description)
    {
        descriptionText.text = description;
    }

    public bool CanBuyAtCost(int cost)
    {
        return PauseMenu.Instance.Score >= cost;
    }

    public void SpendMoney(int cost)
    {
        PauseMenu.Instance.Score -= cost;
    }
}
