using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text requirementsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;

    public void Setup(Equipment equipment, int money, int playerLevel, Action<int> buyCallback)
    {
        headerText.text = equipment.name;
        string requirements = $"Money: {equipment.GetBasePrice()}";

        if (playerLevel < equipment.GetMinimumLevel())
        {
            requirements += $"\nLevel Required: {equipment.GetMinimumLevel()}";
        }
        else
        {
            requirements += $"\nLevel Required: {equipment.GetLevel()}";
        }

        requirementsText.text = requirements;

        if (IsEquipmentOwned(equipment) || equipment.GetEquipmentName() == "House") // extreme laziness
        {
            buyButtonText.text = $"Upgrade - {equipment.GetBasePrice() * equipment.GetLevel()}";
        }
        else
        {
            buyButtonText.text = $"Buy - {equipment.GetBasePrice()}";
        }

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => OnBuy(equipment, money, buyCallback));
    }

    private bool IsEquipmentOwned(Equipment equipment)
    {
        return Player.Instance.GetComponent<PlayerEquipment>().IsEquipmentOwned(equipment);
    }

    private void OnBuy(Equipment equipment, int money, Action<int> buyCallback)
    {
        int price = IsEquipmentOwned(equipment) ? equipment.GetBasePrice() * equipment.GetLevel() : equipment.GetBasePrice();

        buyCallback?.Invoke(price);
    }
}