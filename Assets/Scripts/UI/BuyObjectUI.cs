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
        string requirements = $"";
        int priceAmount = equipment.GetBasePrice() * equipment.GetLevel();

        if (playerLevel < equipment.GetMinimumLevel())
        {
            requirements += $"Level Required: {equipment.GetMinimumLevel()}\nPrice:{priceAmount}";
        }

        if (IsEquipmentOwned(equipment) || equipment.GetEquipmentName() == "House")
        {
            requirements += $"\nCurrent Level: {equipment.GetLevel()}\nPrice:{priceAmount}";
        }

        requirementsText.text = requirements;

        if (IsEquipmentOwned(equipment) || equipment.GetEquipmentName() == "House")
        {
            buyButtonText.text = $"Upgrade - {priceAmount}";
        }
        else
        {
            buyButtonText.text = $"Buy - {priceAmount}";
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