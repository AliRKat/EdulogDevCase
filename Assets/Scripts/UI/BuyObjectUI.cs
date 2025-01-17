using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text requirementsText;
    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text buyButtonText;

    private EquipmentSO equipment;

    public void Setup(Equipment equipment, int money, System.Action buyCallback)
    {

    }

    internal void DisplayRequirements()
    {

    }

    private void OnBuy()
    {
        
    }

    public void UpdateRequirementsText(int newTotal)
    {
        
    }
}