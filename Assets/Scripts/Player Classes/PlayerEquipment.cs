using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public List<Equipment> EquipmentPrefabs;
    private List<Equipment> EquipmentsOwned = new List<Equipment>();
    private Dictionary<string, GameObject> equipmentMeshMap;
    public Equipment Equipped;
    
    public event Action EquipmentUpdated;

    private void Start()
    {
        SubscribeToEquipEvents();
        InitializeMeshReferences();
    }

    public void Equip(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }

        // handle equipment switch
        DeactivateAllEquipmentMeshes();
        ActivateEquipmentMesh(equipment);
        Equipped = equipment;
    }

    public void Unequip(Equipment equipment)
    {
        if (Equipped != null) 
        {
            if (!IsEquipmentOwned(equipment))
            {
                return;
            }

            DeactivateAllEquipmentMeshes();
            Equipped = null;
        }
    }

    public void Drop(Equipment equipment)
    {
        if (!IsEquipmentOwned(equipment))
        {
            return;
        }
        var prefab = EquipmentPrefabs.Find(item => item.GetEquipmentName() == equipment.GetEquipmentName());

        EquipmentsOwned.Remove(prefab);
        DeactivateAllEquipmentMeshes();
        EquipmentUpdated?.Invoke();
        Equipped = null;

        Transform equipmentTransform = prefab.transform;
        Vector3 originalPosition = equipmentTransform.position;
        Quaternion originalRotation = equipmentTransform.rotation;
        GameObject droppedObject = Instantiate(prefab.gameObject, originalPosition, originalRotation);
        droppedObject.transform.parent = null;

        StartCoroutine(AnimateDrop(droppedObject));
    }

    private IEnumerator AnimateDrop(GameObject droppedObject)
    {
        foreach (Transform child in droppedObject.transform)
        {
            child.gameObject.SetActive(true);
        }

        Vector3 startPosition = droppedObject.transform.position;
        Quaternion startRotation = droppedObject.transform.rotation;

        Vector3 targetPosition = new Vector3(startPosition.x, 0f, startPosition.z + 2f);
        Quaternion targetRotation = Quaternion.Euler(-90f, 0f, 0f);

        Vector3 arcPeak = new Vector3(startPosition.x, startPosition.y + 1.5f, startPosition.z + 1f);

        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 position = Vector3.Lerp(
                Vector3.Lerp(startPosition, arcPeak, t),
                Vector3.Lerp(arcPeak, targetPosition, t),
                t
            );
            droppedObject.transform.position = position;

            droppedObject.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        droppedObject.transform.position = targetPosition;
        droppedObject.transform.rotation = targetRotation;
        droppedObject.AddComponent<SelectableObject>();
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

        var prefab = EquipmentPrefabs.Find(item => item.GetEquipmentName() == equipment.GetEquipmentName());

        if (prefab != null)
        {
            EquipmentsOwned.Add(prefab);
            EquipmentUpdated?.Invoke();
        }
        else
        {
            Debug.LogWarning("Prefab not found for " + equipment.GetEquipmentName());
        }
    }

    public bool IsEquipmentOwned(Equipment equipment)
    {
        return EquipmentsOwned.Exists(item => item.GetEquipmentName() == equipment.GetEquipmentName());
    }

    public List<Equipment> GetEquipmentsOwned()
    {
        return EquipmentsOwned;
    }

    public List<Equipment> GetAllEquipments()
    {
        return EquipmentPrefabs;
    }

    public Equipment GetCurrentEquipped()
    {
        return Equipped;
    }

    private void DeactivateAllEquipmentMeshes()
    {
        foreach (var mesh in equipmentMeshMap.Values)
        {
            mesh.SetActive(false);
        }
    }

    private void ActivateEquipmentMesh(Equipment equipment)
    {
        if (equipmentMeshMap.TryGetValue(equipment.GetEquipmentName(), out var mesh))
        {
            mesh.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"Mesh not found for {equipment.GetEquipmentName()}");
        }
    }

    private void InitializeMeshReferences()
    {
        equipmentMeshMap = new Dictionary<string, GameObject>();

        foreach (var equipmentPrefab in EquipmentPrefabs)
        {
            var meshChild = equipmentPrefab.transform.GetChild(0).gameObject;

            if (meshChild != null)
            {
                equipmentMeshMap[equipmentPrefab.GetEquipmentName()] = meshChild;
            }
            else
            {
                Debug.LogWarning($"Mesh child not found for {equipmentPrefab.GetEquipmentName()}");
            }
        }
    }

    private void SubscribeToEquipEvents()
    {
        EquipmentUIManager equipmentUIManager = EquipmentUIManager.Instance;
        equipmentUIManager.equipAction += Equip;
        equipmentUIManager.unequipAction += Unequip;
        equipmentUIManager.dropAction += Drop;
    }

    private void OnDisable()
    {
        EquipmentUIManager equipmentUIManager = EquipmentUIManager.Instance;
        equipmentUIManager.equipAction -= Equip;
        equipmentUIManager.unequipAction -= Unequip;
        equipmentUIManager.dropAction -= Drop;
    }
}