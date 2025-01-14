using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    public int level { get; private set; } = 1;
    public int currentXP { get; private set; }
    public int xpToNextLevel { get; private set; }

    [SerializeField] private int baseXP = 100;
    [SerializeField] private float growthRate = 1.15f;

    public event Action<int> OnLevelUp;

    private void Start()
    {
        LoadPlayerData();
        CalculateXPToNextLevel();
        SubscribeToPlayerEvents();
        Debug.Log($"PlayerLevel: Starting at Level {level} with {currentXP} XP. XP needed to level up: {xpToNextLevel}");
    }

    private void SubscribeToPlayerEvents()
    {
        Player.Instance.OnHarvestFinish += HandleHarvestFinish;
    }

    private void OnDisable()
    {
        Player.Instance.OnHarvestFinish -= HandleHarvestFinish;
    }

    private void HandleHarvestFinish(GameObject obj)
    {
        // gain xp from harvested object
        int gainAmount = obj.GetComponent<Gatherable>().GetHarvestXpAmount();
        AddXP(gainAmount);
        SavePlayerData();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"PlayerLevel: Gained {amount} XP. Current XP: {currentXP}/{xpToNextLevel}");

        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;

        CalculateXPToNextLevel();

        Debug.Log($"PlayerLevel: Level Up! New Level: {level}. Next level requires {xpToNextLevel} XP.");
        OnLevelUp?.Invoke(level);
    }

    private void CalculateXPToNextLevel()
    {
        xpToNextLevel = Mathf.FloorToInt(baseXP * Mathf.Pow(level, growthRate));
    }

    private void SavePlayerData()
    {
        PlayerData playerData = new PlayerData(level, currentXP);
        SaveManager.SavePlayerData(playerData);
    }

    private void LoadPlayerData()
    {
        PlayerData playerData = SaveManager.LoadPlayerData();
        level = playerData.Level;
        currentXP = playerData.CurrentXP;
        Debug.Log($"PlayerLevel: Loaded Player Data: Level {level}, XP {currentXP}");
    }
}