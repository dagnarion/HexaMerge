using System;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField] private LayerMask cell;
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 offSet;
    [SerializeField] private GridController gridController;
    private Slot currentSlot;
    private Camera mainCamera;
    private CellStack currentStack;
    private Vector3 oldPosition;
    private Vector3 currentPoint;
    public event Action<Vector3Int,CellStack> OnPlaced;
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDown();
        if (Input.GetMouseButton(0) && currentStack != null)
            MouseDrag();
        if (Input.GetMouseButtonUp(0) && currentStack != null) 
            MouseUp();
    }

    void MouseDown()
    {
        RaycastHit ray;
        Physics.Raycast(getMouseRay(), out ray, 500, cell);
        if(ray.collider == null) return;
        currentStack = ray.collider.GetComponentInParent<CellStack>();
        currentStack.SetParent(null);
        oldPosition = currentStack.transform.position;
    }

    void MouseDrag()
    {
        RaycastHit ray;
        Physics.Raycast(getMouseRay(), out ray, 500, slotLayer);
        FollowHandle();

        
        if (ray.collider == null)
        {
            currentSlot?.Deselected();
            currentSlot = null;
            return;
        }
        
        Slot slot = gridController.Grid.GetValue(gridController.ConvertWorldPositionToCellPosition(ray.point));
        
        if (currentSlot != slot)
        {
            currentSlot?.Deselected();
            currentSlot = slot;
          if(currentSlot.CanPlaced)  currentSlot?.Selected();
        }
    }

    void MouseUp()
    {
        currentSlot?.Deselected();
        if (currentSlot == null)
        {
            currentStack.transform.position = oldPosition;
            currentSlot = null;
            currentStack = null;
            return;
        }
        if (!currentSlot.CanPlaced)
        {
            currentStack.transform.position = oldPosition;
            currentSlot = null;
            currentStack = null;
            return;
        }

        currentStack.transform.position = currentSlot.transform.position.With(y: currentSlot.transform.position.y + .2f);
        currentSlot?.SetPlacedState(false);
        OnPlaced?.Invoke(gridController.ConvertWorldPositionToCellPosition(currentPoint),currentStack);
        currentStack.UnSelect();
        currentStack.SetParent(currentSlot.transform);
        currentSlot.FillCellStack(currentStack);
        currentSlot = null;
        currentStack = null;
    }


    private void FollowHandle()
    {
        RaycastHit ray;
        Physics.Raycast(getMouseRay(), out ray, 500, groundLayer);
        Vector3 targetPos = ray.point.With(y: 2);
        currentStack.transform.position = targetPos;
        currentPoint = ray.point;
    }


    private Ray getMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
}