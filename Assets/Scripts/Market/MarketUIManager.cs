using System.Collections.Generic;
using UnityEngine;

public class MarketUIManager : MonoBehaviour
{
    [SerializeField] private GameObject marketUI;
    [SerializeField] private GameObject sellPanel;
    [SerializeField] private GameObject buyPanel;
    [SerializeField] private GameObject sellObjectPrefab;
    [SerializeField] private Transform sellPanelContent;
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
        marketUI.SetActive(true);
    }

    public void CloseMarketUI()
    {
        marketUI.SetActive(false);
        Player.Instance.SetPlayerFree();
    }

    public void SwitchMenu()
    {
        if (sellPanel.gameObject.activeInHierarchy)
        {
            buyPanel.gameObject.SetActive(true);
            sellPanel.gameObject.SetActive(false);
        }
        else if (buyPanel.gameObject.activeInHierarchy)
        {
            buyPanel.gameObject.SetActive(false);
            sellPanel.gameObject.SetActive(true);
        }
    }

    private void PopulateSellPanel()
    {
        foreach (Transform child in sellPanelContent)
        {
            Destroy(child.gameObject);
        }

        sellObjects.Clear();

        var inventory = Player.Instance.GetPlayerInventory();
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