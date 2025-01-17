using UnityEngine.UI;
using UnityEngine;

public class EquipmentUIManager : MonoBehaviour
{
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private Transform inventoryContent;
    [SerializeField] private GameObject equipmentObjButton;
    private bool isUIOpen = false;

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

    private void PopulateEquipmentUI()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        var equipments = Player.Instance.GetComponent<PlayerEquipment>().GetEquipmentsOwned();
        foreach (var entry in equipments)
        {
            var equipObj = Instantiate(equipmentObjButton, inventoryContent);
            InventoryObjectUI inventoryUI = equipObj.GetComponent<InventoryObjectUI>();

            if (inventoryUI != null)
            {
                inventoryUI.Setup(entry.GetObjectSprite());
            }
            else
            {
                Debug.LogError("InventoryObjectUI component missing on the equipment button prefab.");
            }
        }
    }
}