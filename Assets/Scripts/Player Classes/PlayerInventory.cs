using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // We can argue about making this class singleton and making other classes listen to this event directly
    // However I think structure-wise, 'Player' class notifiying listeners makes more sense
    public event Action InventoryUpdated;

    private Dictionary<ItemBase, int> inventory = new Dictionary<ItemBase, int>();

    private void Start()
    {
        SubscribeToPlayerEvents();
        LogTheInventory();
    }
    public void OnEnable()
    {
        inventory = SaveManager.LoadInventory();
    }

    private void OnDisable()
    {
        Player.Instance.OnHarvestFinish -= HandleHarvestFinish;
    }

    private void SubscribeToPlayerEvents()
    {
        Player.Instance.OnHarvestFinish += HandleHarvestFinish;
    }

    private void HandleHarvestFinish(GameObject gatheredItem)
    {
        int amountToAdd = gatheredItem.GetComponent<Gatherable>().GetHarvestAmount();
        ItemBase itemToAdd = gatheredItem.GetComponent<Gatherable>().GetGatherItem();
        AddItem(itemToAdd, amountToAdd);
        LogTheInventory();
    }

    public void AddItem(ItemBase item, int quantity)
    {
        if (inventory.ContainsKey(item))
        {
            inventory[item] += quantity;
        }
        else
        {
            inventory.Add(item, quantity);
        }

        InventoryUpdated?.Invoke();
        SaveManager.SaveInventory(inventory);
    }

    public bool RemoveItem(ItemBase item, int quantity)
    {
        if (inventory.ContainsKey(item) && inventory[item] >= quantity)
        {
            inventory[item] -= quantity;
            if (inventory[item] <= 0)
            {
                inventory.Remove(item);
            }

            InventoryUpdated?.Invoke();
            SaveManager.SaveInventory(inventory);
            return true;
        }

        return false;
    }

    // this is a debug method
    public void LogTheInventory()
    {
        foreach (var item in inventory)
        {
            Debug.Log($"PlayerInventory: {item.Key.ItemBaseToString()}, quantity: {item.Value}");
        }
    }
}