using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action<GameObject> OnInteractableClicked;

    private GameObject SelectedObject;
    private Camera mainCamera;
    private GameObject lastHoveredObject;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        HighlightObjectLogic();
    }

    void HighlightObjectLogic()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hoveredObject = hit.collider.gameObject;
            IInteractable interactable = hoveredObject.GetComponent<IInteractable>();

            if (interactable != null)
            {
                Highlight(hoveredObject, interactable);
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
        else
        {
            if (lastHoveredObject != null)
            {
                ResetHighlight();
                lastHoveredObject = null;
            }
        }
    }

    void SelectObject(GameObject obj)
    {
        SelectedObject = obj;
        OnInteractableClicked?.Invoke(obj);
    }

    public GameObject GetSelectedObj()
    {
        return SelectedObject;
    }

    public void ClearSelectedObj()
    {
        SelectedObject = null;
    }

    void Highlight(GameObject hoveredObject, IInteractable interactable)
    {
        if (hoveredObject != lastHoveredObject)
        {
            if (lastHoveredObject != null)
            {
                interactable.ResetHighlight();
            }
            interactable.Highlight();
            lastHoveredObject = hoveredObject;
        }

        if (Input.GetMouseButtonDown(0))
        {
            SelectObject(hoveredObject);
        }
    }

    void ResetHighlight()
    {
        if (lastHoveredObject != null)
        {
            IInteractable interactable = lastHoveredObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.ResetHighlight();
            }
        }
    }
}