using UnityEngine;

public class Gatherable : MonoBehaviour, IInteractable
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

        GameManager.Instance.OnInteractableClicked += HandleClickEvent;
        Player.Instance.OnGatherStarted += HandleGatherStart;
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

    public void HandleClickEvent(GameObject obj)
    {
        if(obj == this.gameObject)
        {
            Debug.Log("Clicked on this gatherable: " + obj.name);
        }
    }

    private void HandleGatherStart(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Debug.Log("Gather has started on this object: " + obj.name);
        }
    }
}
