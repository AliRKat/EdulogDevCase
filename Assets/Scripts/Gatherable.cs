using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
{
    public GatherableSO gatherableData;
    public GatherableStates state;
    public float plowTime;
    public float growTime;
    public float harvestTime;
    public Material highlightMaterial;

    private Material originalMaterial;
    private Renderer _renderer;

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

    private void HandlePlowStart(GameObject obj)
    {

    }
    private void HandlePlowFinish(GameObject obj)
    {

    }
    private void HandleHarvestStart(GameObject obj)
    {

    }
    private void HandleHarvestFinish(GameObject obj)
    {

    }
}