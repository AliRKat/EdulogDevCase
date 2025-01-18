using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    // We can argue about making this class singleton and making other classes listen to this event directly
    // However I think structure-wise, 'Player' class notifiying listeners makes more sense
    public event Action InventoryUpdated;

    internal Dictionary<ItemBase, int> inventory = new Dictionary<ItemBase, int>();

    // Base capacity of the player, starting from a default value
    private int baseCapacity = 50;
    // Bonus capacity is dynamically managed
    private int bonusCapacity = 0;

    private int money = 0;

    // The current total capacity available
    public int CurrentCapacity => baseCapacity + bonusCapacity;

    private void Start()
    {
        SubscribeToPlayerEvents();
        LogTheInventory();
    }

    public void OnEnable()
    {
        LoadData();
    }

    private void OnDisable()
    {
        Player.Instance.OnHarvestFinish -= HandleHarvestFinish;
        Player.Instance.OnEquipmentUpdate -= HandleEquipmentUpdate;
    }

    private void SubscribeToPlayerEvents()
    {
        Player.Instance.OnHarvestFinish += HandleHarvestFinish;
        Player.Instance.OnEquipmentUpdate += HandleEquipmentUpdate;
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
        AddBonus();
        SaveData();
    }

    private void AddBonus()
    {
        Equipment currentEq = Player.Instance.GetComponent<PlayerEquipment>().GetCurrentEquipped();
        if (currentEq.GetBonusType() == BonusTypes.HarvestAmount)
        {
            int bonusAmount = (int)(currentEq.GetLevel() * currentEq.GetBonusMultiplier());
        }
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
            SaveData();
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

    private void HandleEquipmentUpdate()
    {
        int houseLevel = Player.Instance.GetPlayerHouse().GetLevel();
        int bonusCapacity = houseLevel * Player.Instance.GetPlayerHouse().GetUpgradeMultiplier();
        SetBonusCapacity(bonusCapacity);
    }

    // Set the bonus capacity, which can be upgraded by various means (e.g., house upgrade, items)
    public void SetBonusCapacity(int newBonusCapacity)
    {
        bonusCapacity = newBonusCapacity;
        InventoryUpdated?.Invoke();
        SaveData(); // Save after setting bonus capacity
    }

    public void AddMoney(int amount)
    {
        money += amount;
        InventoryUpdated?.Invoke();
        SaveData();
    }

    public bool RemoveMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            InventoryUpdated?.Invoke();
            SaveData();
            return true;
        }

        Debug.LogWarning("PlayerInventory: Not enough money!");
        return false;
    }

    public int GetMoney()
    {
        return money;
    }

    private void SaveData()
    {
        SaveManager.SaveInventory(inventory, money);
    }

    private void LoadData()
    {
        (inventory, money) = SaveManager.LoadInventory();
    }

    // Log inventory for debugging
    public void LogTheInventory()
    {
        Debug.Log("PlayerInventory: Current Inventory:");
        foreach (var item in inventory)
        {
            Debug.Log($" - Item: {item.Key.ItemBaseToString()}, Quantity: {item.Value}");
        }
        Debug.Log($"PlayerInventory: Money = {money}");
    }
}