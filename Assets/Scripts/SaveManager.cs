using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveManager
{
    private static readonly string savePath = Application.persistentDataPath + "/gatherables.json";
    private static readonly string playerDataPath = Application.persistentDataPath + "/playerData.json";
    private static readonly string inventoryPath = Application.persistentDataPath + "/inventory.json";

    public static void SaveInventory(Dictionary<ItemBase, int> inventory)
    {
        List<SerializableItem> items = new List<SerializableItem>();
        foreach (var item in inventory)
        {
            items.Add(new SerializableItem(item.Key.Name, item.Key.Value, item.Value));
        }

        string json = JsonUtility.ToJson(new SerializableItemList(items));
        File.WriteAllText(inventoryPath, json);

        Debug.Log("SaveManager: Inventory saved!");
    }

    public static Dictionary<ItemBase, int> LoadInventory()
    {
        if (!File.Exists(inventoryPath))
        {
            Debug.LogWarning("No inventory data found. Returning an empty inventory.");
            return new Dictionary<ItemBase, int>();
        }

        string json = File.ReadAllText(inventoryPath);
        SerializableItemList itemList = JsonUtility.FromJson<SerializableItemList>(json);

        Dictionary<ItemBase, int> inventory = new Dictionary<ItemBase, int>();
        foreach (var item in itemList.Items)
        {
            inventory.Add(new ItemBase(item.Name, item.Value), item.Quantity);
        }

        return inventory;
    }

    public static void SaveGatherables(List<GatherableData> gatherableDataList)
    {
        string json = JsonUtility.ToJson(new GatherableDataListWrapper { Gatherables = gatherableDataList });
        File.WriteAllText(savePath, json);
        Debug.Log("SaveManager: Gatherables saved!");
    }

    public static List<GatherableData> LoadGatherables()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("SaveManager: No save file found. Returning an empty list.");
            return new List<GatherableData>();
        }

        string json = File.ReadAllText(savePath);
        GatherableDataListWrapper gatherableDataList = JsonUtility.FromJson<GatherableDataListWrapper>(json);
        return gatherableDataList.Gatherables;
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(playerDataPath, json);
        Debug.Log("SaveManager: Player data saved!");
    }

    public static PlayerData LoadPlayerData()
    {
        if (!File.Exists(playerDataPath))
        {
            Debug.LogWarning("SaveManager: No player data file found. Creating default data.");
            return new PlayerData(1, 0);
        }

        string json = File.ReadAllText(playerDataPath);
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
        return playerData;
    }
}

[System.Serializable]
public class SerializableItem
{
    public string Name;
    public int Value;
    public int Quantity;

    public SerializableItem(string name, int value, int quantity)
    {
        Name = name;
        Value = value;
        Quantity = quantity;
    }
}

[System.Serializable]
public class SerializableItemList
{
    public List<SerializableItem> Items;

    public SerializableItemList(List<SerializableItem> items)
    {
        Items = items;
    }
}

[System.Serializable]
public class GatherableData
{
    public string Id;
    public GatherableStates State;
}

[System.Serializable]
public class GatherableDataListWrapper
{
    public List<GatherableData> Gatherables;
}

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int CurrentXP;

    public PlayerData(int level, int currentXP)
    {
        Level = level;
        CurrentXP = currentXP;
    }
}