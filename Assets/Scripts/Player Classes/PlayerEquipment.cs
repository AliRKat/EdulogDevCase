using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public List<Equipment> EquipmentPrefabs;
    private List<Equipment> EquipmentsOwned = new List<Equipment>();
    public Equipment Equipped;
    
    public event Action EquipmentUpdated;

    Equipment questEquipment;
    Quest activeQuest;
    string questEquipmentType;
    int questLevel;

    private void Start()
    {
        SubscribeToEquipEvents();
        LoadEquipments();
        if(!EquipmentsOwned.Contains(Player.Instance.playerHouse))
        {
            EquipmentsOwned.Add(Player.Instance.playerHouse);
        }
    }
    private void Update()
    {
        CheckQuestStatus();
    }

    private void SaveEquipments()
    {
        foreach (var item in EquipmentsOwned)
        {
            PlayerPrefs.SetInt(item.equipmentType.ToString(), 1);
        }
    }

    private void LoadEquipments()
    {
        foreach (var item in EquipmentPrefabs)
        {
            int value = PlayerPrefs.GetInt(item.equipmentType.ToString());
            if (value > 0) 
            {
                EquipmentsOwned.Add((Equipment)item);
            }
        }
        EquipmentUpdated?.Invoke();
    }

    private void RemoveEquipment(EquipmentType type)
    {
        foreach (var item in EquipmentPrefabs)
        {
            if(type == item.equipmentType)
            {
                PlayerPrefs.SetInt(item.equipmentType.ToString(), 0);
            }
        }
    }

    private void Activate(Equipment eq)
    {
        foreach (var item in EquipmentPrefabs)
        {
            if(eq.equipmentType == item.equipmentType)
            {
                item.meshReference.gameObject.SetActive(true);
            }
        }
    }

    private void Deactivate(Equipment eq)
    {
        foreach (var item in EquipmentPrefabs)
        {
            if (eq.equipmentType == item.equipmentType)
            {
                item.meshReference.gameObject.SetActive(false);
            }
        }
    }

    public void Equip(Equipment equipment)
    {
        if (Equipped != null)
        {
            Deactivate(Equipped);
        }

        if (equipment != null)
        {
            Equipped = equipment;
            Activate(equipment);
        }
        SaveEquipments();
    }

    public void Unequip(Equipment equipment)
    {
        if (equipment != null) 
        {
            Equipped = null;
            Deactivate(equipment);
        }
        SaveEquipments();
    }

    public void Drop(Equipment equipment)
    {
        var prefab = EquipmentPrefabs.Find(item => item.GetEquipmentName() == equipment.GetEquipmentName());

        EquipmentsOwned.Remove(prefab);
        RemoveEquipment(equipment.equipmentType);
        Deactivate(equipment);
        EquipmentUpdated?.Invoke();
        Equipped = null;

        Transform equipmentTransform = prefab.transform;
        Vector3 originalPosition = equipmentTransform.position;
        Quaternion originalRotation = equipmentTransform.rotation;
        GameObject droppedObject = Instantiate(prefab.gameObject, originalPosition, originalRotation);
        droppedObject.transform.parent = null;

        SaveEquipments();
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

        DroppedItem droppedItem = droppedObject.AddComponent<DroppedItem>();
        droppedItem.SetPrefabName(droppedObject.GetComponent<Equipment>().GetEquipmentName());
        droppedItem.Save();
    }

    public void Add(EquipmentType type)
    {
        if(EquipmentsOwned.Exists(item => item.GetEquipmentName() == type.ToString()))
        {
            return;
        }

        var prefab = EquipmentPrefabs.Find(item => item.GetEquipmentName() == type.ToString());
        if (prefab != null)
        {
            EquipmentsOwned.Add(prefab);
            EquipmentUpdated?.Invoke();
        }
        else
        {
            Debug.LogWarning("Prefab not found for " + type.ToString());
        }
        SaveEquipments();
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

    private void SubscribeToEquipEvents()
    {
        EquipmentUIManager equipmentUIManager = EquipmentUIManager.Instance;
        equipmentUIManager.equipAction += Equip;
        equipmentUIManager.unequipAction += Unequip;
        equipmentUIManager.dropAction += Drop;
    }

    private void OnDisable()
    {
        SaveEquipments();
        EquipmentUIManager equipmentUIManager = EquipmentUIManager.Instance;
        equipmentUIManager.equipAction -= Equip;
        equipmentUIManager.unequipAction -= Unequip;
        equipmentUIManager.dropAction -= Drop;
    }

    private void OnApplicationQuit()
    {
        SaveEquipments();
    }

    public void EquipmentQuest(Dictionary<string, int> questObjectives, Quest quest)
    {
        foreach (var objective in questObjectives)
        {
            questEquipmentType = objective.Key;
            questLevel = objective.Value;
            activeQuest = quest;
            Debug.Log($"Quest Objective: {questEquipmentType} => Level {questLevel}");

            EquipmentType equipmentType;
            if (Enum.TryParse(questEquipmentType, out equipmentType))
            {
                List<Equipment> allEquipments = GetAllEquipments();
                Equipment equipment = allEquipments.Find(eq => eq.GetEquipmentType() == equipmentType);
                questEquipment = equipment;
            }
            else
            {
                Debug.LogError($"Invalid equipment type: {questEquipmentType}");
            }
        }
    }

    private void CheckQuestStatus()
    {
        if (questEquipment == null)
        {
            return;
        }

        if (questEquipment.GetLevel() == questLevel)
        {
            QuestManager.Instance.QuestCompleted(activeQuest);
        }
    }
}