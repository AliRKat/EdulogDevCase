using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text headerText;
    [SerializeField] private TMP_Text sellAmountText;
    [SerializeField] private TMP_Text totalAmountText;
    [SerializeField] private Slider amountSlider;
    [SerializeField] private Button sellButton;
    [SerializeField] private Button sellAllButton;
    [SerializeField] private TMP_Text sellButtonText;
    [SerializeField] private TMP_Text sellAllButtonText;

    private ItemBase item;
    private int totalAmount;
    private System.Action<ItemBase, int> onSellItem; // Callback for selling an item

    public void Setup(ItemBase item, int amount, System.Action<ItemBase, int> sliderCallback, System.Action<ItemBase, int> sellCallback)
    {
        this.item = item;
        this.totalAmount = amount;
        this.onSellItem = sellCallback;

        headerText.text = item.Name;
        totalAmountText.text = amount.ToString();
        amountSlider.maxValue = amount;
        amountSlider.value = 0;

        UpdateSellAmount(0);
        amountSlider.onValueChanged.AddListener(value =>
        {
            int sellAmount = Mathf.RoundToInt(value);
            UpdateSellAmount(sellAmount);
            sliderCallback?.Invoke(item, sellAmount);
        });

        sellButton.onClick.AddListener(() => OnSell(false));
        sellAllButton.onClick.AddListener(() => OnSell(true));
    }

    internal void UpdateSellAmount(int amount)
    {
        sellAmountText.text = amount.ToString();
        sellButtonText.text = $"Sell {amount} for {item.Value * amount}$";
        sellAllButtonText.text = $"Sell All for {item.Value*amount}$";
    }

    private void OnSell(bool sellAll)
    {
        int amountToSell = sellAll ? totalAmount : Mathf.RoundToInt(amountSlider.value);
        if (amountToSell > 0)
        {
            onSellItem?.Invoke(item, amountToSell);
        }
    }

    public void UpdateTotalAmount(int newTotal)
    {
        totalAmount = newTotal;
        totalAmountText.text = newTotal.ToString();
        amountSlider.maxValue = newTotal;
        if (newTotal == 0)
        {
            amountSlider.value = 0;
            sellButton.interactable = false;
            sellAllButton.interactable = false;
        }
    }
}