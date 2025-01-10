using UnityEngine;

public class GatherableTree : MonoBehaviour, IGatherable
{
    private Material originalMaterial;
    public Material highlightMaterial;

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null)
        {
            originalMaterial = _renderer.material;
        }

        GameManager.Instance.OnGatherableClicked += HandleGatherEvent;

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

    public void HandleGatherEvent(GameObject obj)
    {
        Debug.Log("Tree gathered! " + obj.name);
    }
}
