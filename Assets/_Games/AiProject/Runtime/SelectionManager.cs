using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private LayerMask m_selectableLayers;

    public GameObject LastSelected { get; private set; }

    private void Update()
    {
        var mousePostion = Input.mousePosition;
        var camera = Camera.main;

        var screenToWorldRay = camera.ScreenPointToRay(mousePostion);

        HandleSelection(screenToWorldRay);
        HandleCommanding(screenToWorldRay);
    }

    private void HandleCommanding(Ray screenToWorldRay)
    {
        if (!LastSelected) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(screenToWorldRay, out var hitInfo))
            {
                if (LastSelected.TryGetComponent<AiAgent>(out var agent))
                {
                    agent.CommandToMoveTo(hitInfo.point);
                }
            }
        }
    }

    private void HandleSelection(Ray screenToWorldRay)
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(screenToWorldRay, out var hitInfo, 1000f, m_selectableLayers))
            {
                if (LastSelected)
                {
                    var selectionMarker = LastSelected.GetComponentInChildren<SelectionMarker>();
                    if (selectionMarker) selectionMarker.Deselected();
                }

                LastSelected = hitInfo.collider.gameObject;

                if (LastSelected)
                {
                    var selectionMarker = LastSelected.GetComponentInChildren<SelectionMarker>(true);
                    if (selectionMarker) selectionMarker.Selected();
                }
            }
        }
    }
}
