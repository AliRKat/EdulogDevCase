using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerXPManager : EditorWindow
{
    private int boostAmount = 0;
    // Menu item to open the window
    [MenuItem("Internal Tools/Progress/Add XP")]
    public static void ShowWindow()
    {
        // Creates or shows the editor window
        EditorWindow.GetWindow(typeof(PlayerXPManager), false, "PlayerXPManager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Player XP Manager", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.Label("Note: This functionality is designed for Play-Mode debugging. Please do not use in Editor Mode.", EditorStyles.wordWrappedLabel);

        GUILayout.Space(10);

        // Input fields for item properties
        boostAmount = EditorGUILayout.IntField("Item Quantity", boostAmount);

        // Button to create a new item
        if (GUILayout.Button("Add XP"))
        {
            AddXP(boostAmount);
        }
    }

    private void AddXP(int boost)
    {
        Player.Instance.GetComponent<PlayerLevel>().AddXP(boost);
    }
}
