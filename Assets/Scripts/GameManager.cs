using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event Action<GameObject> OnGatherableClicked;

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
            IGatherable gatherable = hoveredObject.GetComponent<IGatherable>();

            if (gatherable != null)
            {
                Highlight(hoveredObject, gatherable);
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
        OnGatherableClicked?.Invoke(obj);
    }

    public GameObject GetSelectedObj()
    {
        return SelectedObject;
    }

    public void ClearSelectedObj()
    {
        SelectedObject = null;
    }

    void Highlight(GameObject hoveredObject, IGatherable gatherable)
    {
        if (hoveredObject != lastHoveredObject)
        {
            if (lastHoveredObject != null)
            {
                gatherable.ResetHighlight();
            }
            gatherable.Highlight();
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
            IGatherable gatherable = lastHoveredObject.GetComponent<IGatherable>();
            if (gatherable != null)
            {
                gatherable.ResetHighlight();
            }
        }
    }
}