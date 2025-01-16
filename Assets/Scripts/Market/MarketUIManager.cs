using UnityEngine;

public class MarketUIManager : MonoBehaviour
{
    [SerializeField] private GameObject marketUI;

    private void Start()
    {
        Player.Instance.OnMarketEnter += OpenMarketUI;
    }

    private void OnDisable()
    {
        Player.Instance.OnMarketEnter -= OpenMarketUI;
    }

    private void OpenMarketUI()
    {
        marketUI.SetActive(true);
    }

    public void CloseMarketUI()
    {
        marketUI.SetActive(false);
        Player.Instance.SetPlayerFree();
    }
}