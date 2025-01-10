using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Camera mainCamera;
    private Material originalMaterial;
    public Material highlightMaterial;
    private GameObject lastHoveredObject;
    public GameObject SelectedObject { get; set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

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

            if (hoveredObject.CompareTag("Tree"))
            {
                if (hoveredObject != lastHoveredObject)
                {
                    if (lastHoveredObject != null)
                    {
                        ResetHighlight();
                    }
                    HighlightObject(hoveredObject);
                    lastHoveredObject = hoveredObject;
                }
            }
            else if (lastHoveredObject != null)
            {
                ResetHighlight();
                lastHoveredObject = null;
            }

            if (Input.GetMouseButtonDown(0) && hoveredObject.CompareTag("Tree"))
            {
                SelectObject(hoveredObject);
            }
        }
        else if (lastHoveredObject != null)
        {
            ResetHighlight();
            lastHoveredObject = null;
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

    void SelectObject(GameObject obj)
    {
        SelectedObject = obj;
        Debug.Log("Selected Object: " + obj.name);
    }
}