using System.Collections.Generic;
using UnityEngine;

public class MarketUIManager : MonoBehaviour
{
    [SerializeField] private GameObject marketUI;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellObjectPrefab;
    [SerializeField] private GameObject buyObjectPrefab;
    [SerializeField] private Transform sellPanelContent;
    [SerializeField] private Transform buyPanelContent;
    [SerializeField] private GameObject nothingToSellText;
    private Dictionary<ItemBase, SellObjectUI> sellObjects = new Dictionary<ItemBase, SellObjectUI>();

    private void Start()
    {
        Player.Instance.OnMarketEnter += OpenMarketUI;
        Player.Instance.OnInventoryUpdated += PopulateSellPanel;
    }

    private void OnDisable()
    {
        Player.Instance.OnMarketEnter -= OpenMarketUI;
        Player.Instance.OnInventoryUpdated -= PopulateSellPanel;
    }

    private void OpenMarketUI()
    {
        PopulateSellPanel();
        PopulateBuyPanel();
        marketUI.SetActive(true);
    }

    public void CloseMarketUI()
    {
        marketUI.SetActive(false);
        Player.Instance.SetPlayerFree();
    }

    public void SwitchBuyMenu()
    {
        buyPanel.gameObject.SetActive(true);
        sellPanel.gameObject.SetActive(false);
    }

    public void SwitchSellMenu()
    {
        buyPanel.gameObject.SetActive(false);
        sellPanel.gameObject.SetActive(true);
    }

    private void PopulateSellPanel()
    {
        foreach (Transform child in sellPanelContent)
        {
            Destroy(child.gameObject);
        }

        sellObjects.Clear();
        nothingToSellText.SetActive(false);
        var inventory = Player.Instance.GetPlayerInventory();
        if  (inventory.Count > 0)
        {
            foreach (var entry in inventory)
            {
                var item = entry.Key;
                var amount = entry.Value;

                var sellObject = Instantiate(sellObjectPrefab, sellPanelContent);
                var sellObjectUI = sellObject.GetComponent<SellObjectUI>();

                sellObjectUI.Setup(item, amount, OnSliderValueChanged, OnSellItem);
                sellObjects[item] = sellObjectUI;
            }
        }
        else
        {
            nothingToSellText.SetActive(true);
        }
        
    }

    private void PopulateBuyPanel()
    {
        // Paneldeki eski öðeleri temizle
        foreach (Transform child in buyPanelContent)
        {
            Destroy(child.gameObject);
        }

        PlayerEquipment playerEquipment = Player.Instance.GetComponent<PlayerEquipment>();
        var ownedEquipments = playerEquipment.GetEquipmentsOwned();
        var allEquipments = playerEquipment.GetAllEquipments();

        foreach (var equipment in allEquipments)
        {
            if (ownedEquipments.Contains(equipment))
            {
                var buyObject = Instantiate(buyObjectPrefab, buyPanelContent);
                var buyObjectUI = buyObject.GetComponent<BuyObjectUI>();

                buyObjectUI.Setup(
                    equipment,
                    Player.Instance.GetPlayerMoney(),
                    Player.Instance.GetPlayerLevel(),
                    (price) => OnUpgradeItem(equipment, price) // upgrade
                );
            }
            else
            {
                var buyObject = Instantiate(buyObjectPrefab, buyPanelContent);
                var buyObjectUI = buyObject.GetComponent<BuyObjectUI>();

                buyObjectUI.Setup(
                    equipment,
                    Player.Instance.GetPlayerMoney(),
                    Player.Instance.GetPlayerLevel(),
                    (price) => OnBuyItem(equipment, price) // buy
                );
            }
        }
    }

    private void OnBuyItem(Equipment equipment, int price)
    {
        if (Player.Instance.GetPlayerLevel() >= equipment.GetMinimumLevel() && Player.Instance.SpendMoney(price))
        {
            PlayerEquipment playerEquipment = Player.Instance.GetComponent<PlayerEquipment>();
            playerEquipment.Add(equipment);
        }
        PopulateBuyPanel();
    }

    private void OnUpgradeItem(Equipment equipment, int price)
    {
        if(Player.Instance.GetPlayerLevel() >= equipment.GetLevel() && Player.Instance.SpendMoney(price))
        {
            equipment.LevelUp();
        }
        PopulateBuyPanel();
    }

    private void OnSellItem(ItemBase item, int amount)
    {
        var playerInventory = Player.Instance.GetComponent<PlayerInventory>();
        bool success = playerInventory.RemoveItem(item, amount);

        if (success)
        {
            if (sellObjects.ContainsKey(item))
            {
                int remainingAmount = playerInventory.inventory[item];
                sellObjects[item].UpdateTotalAmount(remainingAmount);
            }

            playerInventory.AddMoney(item.Value * amount);
        }
    }

    private void OnSliderValueChanged(ItemBase item, int sellAmount)
    {
        if (sellObjects.ContainsKey(item))
        {
            sellObjects[item].UpdateSellAmount(sellAmount);
        }
    }
}