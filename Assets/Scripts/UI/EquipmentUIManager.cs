using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class EquipmentUIManager : MonoBehaviour
{
    public static EquipmentUIManager Instance { get; private set; }
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject equipmentObjButton;
    private List<Equipment> equipmentList;
    private Equipment pickedEquipment;
    private bool isUIOpen = false;

    [SerializeField] private Image detailsScrenSprite;
    [SerializeField] private TMP_Text detailsScrenDescription;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unequipButton;
    [SerializeField] private Button dropButton;

    public System.Action<Equipment> equipAction;
    public System.Action<Equipment> unequipAction;
    public System.Action<Equipment> dropAction;

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
    }

    private void Start()
    {
        Player.Instance.OnEquipmentUpdate += HandleEquipmentUpdate;
    }

    private void OnDisable()
    {
        Player.Instance.OnEquipmentUpdate -= HandleEquipmentUpdate;
    }

    private void HandleEquipmentUpdate()
    {
        PopulateEquipmentUI();
        ClearEquipmentDetailsScreen();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isUIOpen)
            {
                CloseEquipmentUI();
            }
            else
            {
                OpenEquipmentUI();
            }
        }
    }

    public void OpenEquipmentUI()
    {
        PopulateEquipmentUI();
        ClearEquipmentDetailsScreen();
        equipmentUI.SetActive(true);
        Player.Instance.SetPlayerBusy();
        isUIOpen = true;
    }

    public void CloseEquipmentUI()
    {
        equipmentUI.SetActive(false);
        Player.Instance.SetPlayerFree();
        isUIOpen = false;
    }

    public void Equip()
    {
        equipAction?.Invoke(pickedEquipment);
    }

    public void Unequip()
    {
        unequipAction?.Invoke(pickedEquipment);
        if (Player.Instance.GetComponent<PlayerEquipment>().GetCurrentEquipped() != null)
        {
            ClearEquipmentDetailsScreen();
        } 
    }

    public void Drop()
    {
        dropAction?.Invoke(pickedEquipment);
        if (Player.Instance.GetComponent<PlayerEquipment>().GetCurrentEquipped() != null)
        {
            ClearEquipmentDetailsScreen();
        }
    }

    private void PopulateEquipmentUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        equipmentList = Player.Instance.GetComponent<PlayerEquipment>().GetEquipmentsOwned();
        foreach (var entry in equipmentList)
        {
            var equipObj = Instantiate(equipmentObjButton, inventoryContent);
            EquipmentObjectUI inventoryUI = equipObj.GetComponent<EquipmentObjectUI>();

            if (inventoryUI != null)
            {
                inventoryUI.Setup(entry, UpdateEquipmentDetailsScreen);
            }
            else
            {
                Debug.LogError("EquipmentObjectUI component missing on the equipment button prefab.");
            }
        }
    }

    private void UpdateEquipmentDetailsScreen(Equipment equipment)
    {
        detailsScrenSprite.sprite = equipment.GetObjectSprite();
        detailsScrenDescription.text = equipment.GetEquipmentDescription();
        pickedEquipment = equipment;
    }

    private void ClearEquipmentDetailsScreen()
    {
        detailsScrenSprite.sprite = null;
        detailsScrenDescription.text = "Click on an equipment to see the details and equip!";
        pickedEquipment = null;
    }
}