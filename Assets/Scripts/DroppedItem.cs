using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public string ItemID;
    public string PrefabName;
    private Vector3 position;
    private void Awake()
    {
        if (string.IsNullOrEmpty(ItemID))
        {
            ItemID = System.Guid.NewGuid().ToString();
        }
    }

    public void SetPrefabName(string name)
    {
        PrefabName = name;
    }

    public void Save()
    {
        var droppedItems = SaveManager.LoadDroppedItems();
        droppedItems.Add(new DroppedItemData
        {
            itemID = ItemID,
            prefabName = PrefabName,
            position = transform.position
        });
        SaveManager.SaveDroppedItems(droppedItems);
    }

    public void Pickup()
    {
        SaveManager.RemoveDroppedItem(ItemID);
        Destroy(gameObject);
        Debug.Log($"DroppedItem: Item [{ItemID}] picked up and removed from the save file.");
    }
}