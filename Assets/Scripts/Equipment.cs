using UnityEngine;

public class Equipment : MonoBehaviour
{
    public UpgradeableSO equipmentData;
    public GameObject meshReference;

    internal int level = 1;

    private string equipmentName;
    private string equipmentDescription;
    private BonusTypes bonus;
    private float bonusMultiplier;
    private int minimumLevel;
    private int basePrice;
    private int upgradeMultiplier;
    private Sprite objectSprite;

    private void Start()
    {
        equipmentName = equipmentData.equipmentName;
        equipmentDescription = equipmentData.equipmentDescription;
        bonus = equipmentData.bonus;
        bonusMultiplier = equipmentData.bonusMultiplier;
        minimumLevel = equipmentData.minimumLevel;
        basePrice = equipmentData.basePrice;
        upgradeMultiplier = equipmentData.upgradeMultiplier;
        objectSprite = equipmentData.objectSprite;
    }

    public string GetEquipmentName()
    {
        return equipmentName;
    }

    public string GetEquipmentDescription()
    {
        return equipmentDescription;
    }

    public BonusTypes GetBonusType()
    {
        return bonus;
    }

    public float GetBonusMultiplier()
    {
        return bonusMultiplier;
    }

    public int GetMinimumLevel() 
    {
        return minimumLevel;
    }

    public int GetBasePrice()
    {
        return basePrice;
    }

    public int GetUpgradeMultiplier()
    {
        return upgradeMultiplier;
    }

    public int GetLevel()
    {
        return level;
    }

    public Sprite GetObjectSprite()
    {
        return objectSprite;
    }

    public void LevelUp()
    {
        level++;
    }
}