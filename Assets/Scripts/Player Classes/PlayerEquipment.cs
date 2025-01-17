using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public List<Equipment> EquipmentPrefabs;
    private List<Equipment> EquipmentsOwned = new List<Equipment>();
    public Equipment Equipped;
    
    public event Action EquipmentUpdated;

    private void Start()
    {
        EquipmentsOwned = EquipmentPrefabs;
    }

    public void Equip(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }

        // handle equipment switch
        DeactivateAllEquipmentPrefabs();
        ActivateEquipmentPrefab(equipment);
        Equipped = equipment;
    }

    public void Unequip(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }

        // handle equipment switch
        DeactivateAllEquipmentPrefabs();
        Equipped = null;
    }

    public void Drop(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }

        EquipmentsOwned.Remove(equipment);
        EquipmentUpdated?.Invoke();
        // handle equipment switch
    }

    public void Upgrade(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }

        // handle equipment level switch
        equipment.LevelUp();
    }

    public void Add(Equipment equipment)
    {
        if (IsEquipmentOwned(equipment))
        {
            return;
        }
        EquipmentsOwned.Add(equipment);
        EquipmentUpdated?.Invoke();
    }

    public List<Equipment> GetEquipmentsOwned()
    {
        return EquipmentsOwned;
    }

    private bool IsEquipmentOwned(Equipment equipment)
    {
        return EquipmentsOwned.Exists(item => item.GetEquipmentName() == equipment.GetEquipmentName());
    }

    private void DeactivateAllEquipmentPrefabs()
    {
        foreach (var equipmentPrefab in EquipmentPrefabs)
        {
            equipmentPrefab.gameObject.SetActive(false);
        }
    }

    private void ActivateEquipmentPrefab(Equipment equipment)
    {
        var prefab = EquipmentPrefabs.Find(item => item.GetEquipmentName() == equipment.GetEquipmentName());

        if (prefab != null)
        {
            prefab.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Prefab not found for " + equipment.GetEquipmentName());
        }
    }
}