using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // We can argue about making this class singleton and making other classes listen to this event directly
    // However I think structure-wise, 'Player' class notifiying listeners makes more sense
    public event Action InventoryUpdated;

    private Dictionary<ItemBase, int> inventory = new Dictionary<ItemBase, int>();

    // Base capacity of the player, starting from a default value
    private int baseCapacity = 50; // Default starting value, can be changed with level
    // Bonus capacity is dynamically managed
    private int bonusCapacity = 0;

    // The current total capacity available
    public int CurrentCapacity => baseCapacity + bonusCapacity;

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

        // if adding only one item like an equipment, check then add
        if (amountToAdd == 1)
        {
            if (CanAddItem(itemToAdd, amountToAdd))
            {
                AddItem(itemToAdd, amountToAdd);
                LogTheInventory();
            }
            else
            {
                Debug.LogWarning("PlayerInventory: Not enough space in inventory!");
            }
        }
        else
        {
            int remainingAmount = amountToAdd;
            while (remainingAmount > 0)
            {
                if (CanAddItem(itemToAdd, 1))
                {
                    AddItem(itemToAdd, 1);
                    remainingAmount--;
                }
                else
                {
                    Debug.LogWarning($"PlayerInventory: Not enough space to add {remainingAmount} items. Only {remainingAmount - 1} could be added.");
                    break;
                }
            }
        }
    }

    // Adds an item to the inventory
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

    // Removes an item from the inventory
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

    // This method checks if the player has enough capacity to add a new item
    public bool CanAddItem(ItemBase item, int quantity)
    {
        int totalQuantity = GetTotalInventoryQuantity() + quantity;

        // Check if the total quantity exceeds the current capacity
        return totalQuantity <= CurrentCapacity;
    }

    // Get the current total item quantity in the inventory
    public int GetTotalInventoryQuantity()
    {
        int total = 0;
        foreach (var item in inventory)
        {
            total += item.Value;
        }

        return total;
    }

    // Set the base capacity based on the player's level
    public void SetBaseCapacity(int newBaseCapacity)
    {
        baseCapacity = newBaseCapacity;
        InventoryUpdated?.Invoke();
        SaveManager.SaveInventory(inventory); // Save after setting base capacity
    }

    // Set the bonus capacity, which can be upgraded by various means (e.g., house upgrade, items)
    public void SetBonusCapacity(int newBonusCapacity)
    {
        bonusCapacity = newBonusCapacity;
        InventoryUpdated?.Invoke();
        SaveManager.SaveInventory(inventory); // Save after setting bonus capacity
    }

    // Log inventory for debugging
    public void LogTheInventory()
    {
        foreach (var item in inventory)
        {
            Debug.Log($"PlayerInventory: {item.Key.ItemBaseToString()}, quantity: {item.Value}");
        }
    }
}