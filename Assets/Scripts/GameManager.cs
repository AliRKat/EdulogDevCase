using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Camera mainCamera;
    private Material originalMaterial;
    public Material highlightMaterial;
    private GameObject lastHoveredObject;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredObject = hit.collider.gameObject;

            if (hoveredObject != lastHoveredObject && gameObject.CompareTag("Tree"))
            {
                if (lastHoveredObject != null)
                {
                    ResetHighlight();
                }
                HighlightObject(hoveredObject);
                lastHoveredObject = hoveredObject;
            }
        }
        else
        {
            if (lastHoveredObject != null)
            {
                ResetHighlight();
                lastHoveredObject = null;
            }
        }
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            renderer.material = highlightMaterial;
        }
    }

    void ResetHighlight()
    {
        if (lastHoveredObject != null)
        {
            Renderer renderer = lastHoveredObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = originalMaterial;
            }
        }
    }
}