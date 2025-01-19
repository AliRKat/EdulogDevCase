using System;
using System.Collections.Generic;

public class Quest
{
    public string QuestID { get; private set; }
    public int MoneyReward { get; set; }
    public int XpReward { get; set; }
    public Dictionary<string, int> Objectives { get; set; }
    public bool IsCompleted { get; set; }

    public Quest(int money, int xp)
    {
        QuestID = GenerateUniqueQuestID();
        MoneyReward = money;
        XpReward = xp;
        IsCompleted = false;
    }

    private string GenerateUniqueQuestID()
    {
        return Guid.NewGuid().ToString();
    }

    public bool CheckCompletion()
    {
        return IsCompleted;
    }
}