using UnityEngine;
using UnityEngine.UI;

public class InventoryObjectUI : MonoBehaviour
{
    [SerializeField] Image image;

    public void Setup(Sprite sprite)
    {
        if (sprite != null)
        {
            image.sprite = sprite;
        }
        else
        {
            Debug.LogWarning("Sprite is null!");
        }
    }
}
