using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "", menuName = "Equipment")]
public class UpgradeableSO : ScriptableObject
{
    public string equipmentName;
    public string equipmentDescription;
    public BonusTypes bonus;
    public float bonusMultiplier;
    public int minimumLevel;
    public int basePrice;
    public int upgradeMultiplier;

    public Sprite objectSprite;
}