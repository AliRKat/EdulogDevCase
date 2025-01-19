using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public Quest activeQuest;
    public EquipmentType[] EquipmentTypesToGiveQuest;
    private PlayerInventory playerInventory;
    private PlayerLevel playerLevel;
    private PlayerEquipment playerEquipment;
    private bool isQuestInProgress = false;

    private int baseMoney = 50;
    private int baseXP = 50;
    private int questTypeIndex = 0;

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
        playerInventory = Player.Instance.GetComponent<PlayerInventory>();
        playerLevel = Player.Instance.GetComponent<PlayerLevel>();
        playerEquipment = Player.Instance.GetComponent<PlayerEquipment>();
        GenerateNewQuest();
    }

    public void QuestCompleted(Quest quest)
    {
        Debug.Log("Quest is completed");
        isQuestInProgress = false;
        playerInventory.AddMoney(quest.MoneyReward);
        playerLevel.AddXP(quest.XpReward);
        GenerateNewQuest();
    }

    private void GenerateNewQuest()
    {
        if (!isQuestInProgress)
        {
            int moneyReward = GenerateMoneyReward();
            int xpReward = GenerateXPReward();
            Quest quest = new Quest(moneyReward, xpReward);

            Dictionary<string, int> questObjectives = GenerateQuestObjective(quest);
            quest.Objectives = questObjectives;
            activeQuest = quest;
            isQuestInProgress = true;

            questTypeIndex = (questTypeIndex + 1) % 3;
        }
    }

    private int GenerateMoneyReward()
    {
        return baseMoney * playerLevel.level;
    }

    private int GenerateXPReward()
    {
        return baseXP * playerLevel.level;
    }

    private Dictionary<string, int> GenerateQuestObjective(Quest quest)
    {
        Dictionary<string, int> questObjectives = new Dictionary<string, int>();

        switch (questTypeIndex)
        {
            case 0: // shovel
                GenerateEquipmentQuest(EquipmentTypesToGiveQuest[0], ref questObjectives, quest);
                break;

            case 1: // scythe
                GenerateEquipmentQuest(EquipmentTypesToGiveQuest[1], ref questObjectives, quest);
                break;

            case 2: // pitchfork
                GenerateEquipmentQuest(EquipmentTypesToGiveQuest[2], ref questObjectives, quest);
                break;
        }

        return questObjectives;
    }

    private void GenerateEquipmentQuest(EquipmentType equipmentType, ref Dictionary<string, int> questObjectives, Quest quest)
    {
        questObjectives.Add(equipmentType.ToString(), playerLevel.level + 1);
        playerEquipment.EquipmentQuest(questObjectives, quest);
    }
}