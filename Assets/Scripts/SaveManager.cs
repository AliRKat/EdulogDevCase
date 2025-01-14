using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveManager
{
    private const string InventoryKey = "PlayerInventory";
    private static readonly string savePath = Application.persistentDataPath + "/gatherables.json";

    public static void SaveInventory(Dictionary<ItemBase, int> inventory)
    {
        List<SerializableItem> items = new List<SerializableItem>();
        foreach (var item in inventory)
        {
            items.Add(new SerializableItem(item.Key.Name, item.Key.Value, item.Value));
        }

        string json = JsonUtility.ToJson(new SerializableItemList(items));
        PlayerPrefs.SetString(InventoryKey, json);
        PlayerPrefs.Save();
    }

    public static Dictionary<ItemBase, int> LoadInventory()
    {
        if (!PlayerPrefs.HasKey(InventoryKey))
            return new Dictionary<ItemBase, int>();

        string json = PlayerPrefs.GetString(InventoryKey);
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