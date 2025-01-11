using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
{
    public GatherableSO gatherableData;
    public Material highlightMaterial;

    private Material originalMaterial;
    private Renderer _renderer;
    private GatherableStates state;
    private float plowTime;
    private float growTime;
    private float harvestTime;
    private int harvestXpAmount;
    private int harvestAmount;

    private void OnDisable()
    {
        Player.Instance.OnPlowStart -= HandlePlowStart;
        Player.Instance.OnPlowFinish -= HandlePlowFinish;
        Player.Instance.OnHarvestStart -= HandleHarvestStart;
        Player.Instance.OnHarvestFinish -= HandleHarvestFinish;
    }

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            originalMaterial = _renderer.material;
        }

        plowTime = gatherableData.plowTime;
        growTime = gatherableData.growTime;
        harvestTime = gatherableData.harvestTime;
        state = gatherableData.state;
        harvestXpAmount = gatherableData.harvestXPAmount;
        harvestAmount = gatherableData.harvestAmount;

        SubscribeToPlayerEvents();
    }
    public void SubscribeToPlayerEvents()
    {
        Player.Instance.OnPlowStart += HandlePlowStart;
        Player.Instance.OnPlowFinish += HandlePlowFinish;
        Player.Instance.OnHarvestStart += HandleHarvestStart;
        Player.Instance.OnHarvestFinish += HandleHarvestFinish;
    }
    public void Highlight()
    {
        if (_renderer != null)
        {
            _renderer.material = highlightMaterial;
        }
    }

    public void ResetHighlight()
    {
        if (_renderer != null)
        {
            _renderer.material = originalMaterial;
        }
    }
    #region Getters
    public float GetPlowTime()
    {
        return plowTime;
    }
    public float GetGrowTime()
    {
        return growTime;
    }
    public float GetHarvestTime()
    {
        return harvestTime;
    }
    public float GetHarvestXpAmount()
    {
        return harvestXpAmount;
    }
    public float GetHarvestAmount()
    {
        return harvestAmount;
    }
    public GatherableStates GetCurrentState()
    {
        return state;
    }
    #endregion
    #region EventHandlers
    // Listens 'Player' class' OnPlowStart event 
    private void HandlePlowStart(GameObject obj)
    {
        if (obj == this.gameObject) 
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling plow start");
        }
    }
    // Listens 'Player' class' OnPlowFinish event 
    private void HandlePlowFinish(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling plow finish");
        }
    }
    // Listens 'Player' class' OnHarvestStart event 
    private void HandleHarvestStart(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling harvest start");
        }
    }
    // Listens 'Player' class' OnHarvestFinish event 
    private void HandleHarvestFinish(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            // FX and other visuals related to Gatherable will be handled here
            Debug.Log("Gatherable: Handling harvest finish");
        }
    }
    #endregion
}