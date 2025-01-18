using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentObjectUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Button button;

    private Equipment assignedEquipment;
    private System.Action<Equipment> onButtonClick;
    public void Setup(Equipment equipment, System.Action<Equipment> clickCallback)
    {
        assignedEquipment = equipment;
        Sprite sprite = equipment.GetObjectSprite();
        if (sprite != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Sprite is null!");
        }

        this.onButtonClick = clickCallback;
        button.onClick.AddListener(() => OnClick());
    }

    private void OnClick() 
    {
        onButtonClick?.Invoke(assignedEquipment);
    }
}