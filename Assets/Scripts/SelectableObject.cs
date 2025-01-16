using UnityEngine;

public class SelectableObject : MonoBehaviour, IInteractable
{
    public static Material DefaultHighlightMaterial;
    [SerializeField] Transform predefinedPosition;
    [SerializeField] Renderer _renderer;
    private Material originalMaterial;

    void Start()
    {
        if (_renderer == null)
        {
            _renderer = GetComponentInChildren<Renderer>();
        }

        if (_renderer != null)
        {
            originalMaterial = _renderer.material;
        }
        else
        {
            Debug.LogWarning($"SelectableObject: No Renderer found on {gameObject.name} or its children!");
        }
    }

    public void Highlight()
    {
        if (_renderer != null)
        {
            _renderer.material = DefaultHighlightMaterial;
        }
    }

    public void ResetHighlight()
    {
        if (_renderer != null)
        {
            _renderer.material = originalMaterial;
        }
    }

    public Transform ReturnPredefinedTransform()
    {
        if(predefinedPosition)
        {
            return predefinedPosition;
        }
        else
        {
            return gameObject.transform;
        }
    }
}