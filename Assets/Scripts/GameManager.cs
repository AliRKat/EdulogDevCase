using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<GameObject> OnInteractableClicked;
    [SerializeField] Material DefaultHighlightMaterial;

    private GameObject SelectedObject;
    private Camera mainCamera;
    private GameObject lastHoveredObject;

    public List<GameObject> itemPrefabs;
    private Dictionary<string, GameObject> prefabDictionary;
    public TMP_Text generalText;
    [SerializeField] private GameObject EscapeMenu;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        mainCamera = Camera.main;
        SelectableObject.DefaultHighlightMaterial = DefaultHighlightMaterial;
    }

    private void InitializePrefabDictionary()
    {
        prefabDictionary = new Dictionary<string, GameObject>();

        foreach (var item in itemPrefabs)
        {
            Equipment equipment = item.GetComponent<Equipment>();
            if (equipment != null)
            {
                string equipmentType = equipment.GetEquipmentType().ToString();

                if (!prefabDictionary.ContainsKey(equipmentType))
                {
                    prefabDictionary.Add(equipmentType, item);
                }
                else
                {
                    Debug.LogWarning($"Prefab dictionary already contains an entry for equipment type: {equipmentType}");
                }
            }
            else
            {
                Debug.LogError($"Item {item.name} does not have an Equipment component!");
            }
        }
    }

    private void Start()
    {
        InitializePrefabDictionary();
        var droppedItems = SaveManager.LoadDroppedItems();
        if (droppedItems.Count > 0)
        {
            foreach (var itemData in droppedItems)
            {
                Debug.Log($"Attempting to find prefab for: {itemData.prefabName}");

                if (prefabDictionary.TryGetValue(itemData.prefabName, out var itemPrefab))
                {

                    Quaternion desiredRotation = Quaternion.Euler(-90, 0, 0);

                    GameObject itemInstance = Instantiate(itemPrefab, itemData.position, desiredRotation);
                    DroppedItem droppedItem = itemInstance.AddComponent<DroppedItem>();
                    droppedItem.AddComponent<SelectableObject>();
                    droppedItem.ItemID = itemData.itemID;
                    droppedItem.PrefabName = itemData.prefabName;
                    Debug.Log($"Successfully instantiated prefab: {itemData.prefabName}");
                }
                else
                {
                    Debug.LogWarning($"Prefab '{itemData.prefabName}' not found in the dictionary! Available keys: {string.Join(", ", prefabDictionary.Keys)}");
                }
            }
        }
    }

    void Update()
    {
        HighlightObjectLogic();
        UpdateGeneralText();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleEscapeMenu();
        }
    }

    private void ToggleEscapeMenu()
    {
        if (EscapeMenu != null)
        {
            bool isActive = EscapeMenu.activeSelf;
            if (!isActive) 
            {
                Player.Instance.SetPlayerBusy();
            }
            else
            {
                Player.Instance.SetPlayerFree();
            }
            EscapeMenu.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("Target object is not assigned in GameManager!");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    void HighlightObjectLogic()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredObject = hit.collider.gameObject;
            IInteractable interactable = hoveredObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Highlight(hoveredObject, interactable);
            }
            else
            {
                if (lastHoveredObject != null)
                {
                    ResetHighlight();
                    lastHoveredObject = null;
                }
            }
        }
        else
        {
            if (lastHoveredObject != null)
            {
                ResetHighlight();
                lastHoveredObject = null;
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        SelectedObject = obj;
        OnInteractableClicked?.Invoke(obj);
    }

    void UpdateGeneralText()
    {
        Quest activeQuest = QuestManager.Instance.activeQuest;
        string objectivesText = "";
        var inventory = Player.Instance.GetPlayerInventory();

        // Add quest objectives
        foreach (var objective in activeQuest.Objectives)
        {
            objectivesText += $"{objective.Key}: Level {objective.Value}\n";
        }

        // Add inventory items
        string inventoryText = "Current Inventory:\n";
        foreach (var item in inventory)
        {
            inventoryText += $"{item.Key.Name}: {item.Value}\n";
        }

        // Update general text
        generalText.text = $"Level: {Player.Instance.GetPlayerLevel()}\nMoney: {Player.Instance.GetPlayerMoney()}\n" +
                           $"Quest Objectives:\n{objectivesText}" +
                           $"{inventoryText}";
    }


    public bool IsObjectSelected()
    {
        if (!SelectedObject)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public SelectableObject GetSelectedObj()
    {
        return SelectedObject.GetComponent<SelectableObject>();
    }

    public void ClearSelectedObj()
    {
        SelectedObject = null;
    }

    void Highlight(GameObject hoveredObject, IInteractable interactable)
    {
        if (hoveredObject != lastHoveredObject)
        {
            if (lastHoveredObject != null)
            {
                interactable.ResetHighlight();
            }
            interactable.Highlight();
            lastHoveredObject = hoveredObject;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectObject(hoveredObject);
        }
    }

    void ResetHighlight()
    {
        if (lastHoveredObject != null)
        {
            IInteractable interactable = lastHoveredObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ResetHighlight();
            }
        }
    }

    private void OnApplicationQuit()
    {
        var allGatherables = FindObjectsOfType<Gatherable>();
        List<GatherableData> gatherableDataList = new List<GatherableData>();

        foreach (var gatherable in allGatherables)
        {
            gatherableDataList.Add(new GatherableData
            {
                Id = gatherable.GetUniqueId(),
                State = gatherable.GetCurrentState()
            });
        }

        SaveManager.SaveGatherables(gatherableDataList);
    }
}