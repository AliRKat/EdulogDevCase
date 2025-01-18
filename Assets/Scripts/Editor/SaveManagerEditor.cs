using UnityEditor;
using UnityEngine;

public class SaveManagerEditor : EditorWindow
{
    // Menu item to open the window
    [MenuItem("Internal Tools/SaveManager/Clear Data")]
    public static void ShowWindow()
    {
        // Creates or shows the editor window
        EditorWindow.GetWindow(typeof(SaveManagerEditor), false, "SaveManager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Save Manager - Clear Data", EditorStyles.boldLabel);

        // Button for clearing inventory
        if (GUILayout.Button("Clear Inventory"))
        {
            ClearInventoryData();
        }

        // Button for clearing XP progression
        if (GUILayout.Button("Clear XP Progression"))
        {
            ClearXPProgressionData();
        }

        // Button for clearing gatherable state
        if (GUILayout.Button("Clear Gatherable State"))
        {
            ClearGatherableStateData();
        }

        // Button for clearing gatherable state
        if (GUILayout.Button("Clear Equipment Data"))
        {
            ClearEquipmentData();
        }

        // Button for clearing gatherable state
        if (GUILayout.Button("Clear All Data"))
        {
            ClearAllData();
        }
    }

    // Clear inventory data
    private void ClearInventoryData()
    {
        SaveManager.ClearInventoryData();
    }

    // Clear XP progression data
    private void ClearXPProgressionData()
    {
        SaveManager.ClearXPProgressionData();
    }

    // Clear gatherable state data
    private void ClearGatherableStateData()
    {
        SaveManager.ClearGatherableStateData();
    }

    // Clear equipment data
    private void ClearEquipmentData()
    {
        SaveManager.ClearEquipmentData();
    }

    private void ClearAllData()
    {
        SaveManager.ClearInventoryData();
        SaveManager.ClearXPProgressionData();
        SaveManager.ClearGatherableStateData();
        SaveManager.ClearEquipmentData();
    }
}