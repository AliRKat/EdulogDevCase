using UnityEditor;
using UnityEngine;

public class InventoryManagerEditor : EditorWindow
{
    private string itemName = "";
    private int itemValue = 0;
    private int itemQuantity = 0;

    // Menu item to open the window
    [MenuItem("Internal Tools/Inventory/Add Item to Inventory")]
    public static void ShowWindow()
    {
        // Creates or shows the editor window
        EditorWindow.GetWindow(typeof(InventoryManagerEditor), false, "Inventory Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Inventory Manager - Create Item", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("Note: This functionality is designed for Play-Mode usage. Please do not use in Editor Mode", EditorStyles.wordWrappedLabel);

        GUILayout.Space(10);

        // Input fields for item properties
        itemName = EditorGUILayout.TextField("Item Name", itemName);
        itemValue = EditorGUILayout.IntField("Item Value", itemValue);
        itemQuantity = EditorGUILayout.IntField("Item Quantity", itemQuantity);

        // Button to create a new item
        if (GUILayout.Button("Create Item"))
        {
            CreateItem(itemName, itemValue, itemQuantity);
        }
    }

    // Method to create an item and add to PlayerInventory
    private void CreateItem(string name, int value, int quantity)
    {
        // Check if the provided values are valid
        if (string.IsNullOrEmpty(itemName) || itemValue <= 0)
        {
            Debug.LogWarning("InventoryManagerEditor: Invalid input values. Item name must not be empty and value must be greater than zero.");
            return;
        }

        // Create a new ItemBase object
        ItemBase newItem = new ItemBase(itemName, itemValue);

        // Log the new item for now
        Debug.Log($"InventoryManagerEditor: Created Item: {newItem.ItemBaseToString()}");

        // Add item to PlayerInventory
        AddItemToInventory(newItem, itemQuantity);
    }

    // Add item to the PlayerInventory's inventory
    private void AddItemToInventory(ItemBase item, int quantity)
    {
        // Get the PlayerInventory
        PlayerInventory playerInventory = Player.Instance.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            playerInventory.AddItem(item, quantity);
            Debug.Log($"InventoryManagerEditor: Added {item.ItemBaseToString()} to inventory.");
        }
        else
        {
            Debug.LogError("InventoryManagerEditor: PlayerInventory not found in the scene.");
        }
    }
}