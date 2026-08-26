using System;
using System.Collections.Generic;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    [SerializeField] private LayerMask slotLayer;
    [SerializeField] private LayerMask groundLayer;
    private Slot currentSlot;
    private Camera mainCamera;
    private Dictionary<Collider, Slot> slotHolder = new Dictionary<Collider, Slot>();
    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDown();
        if (Input.GetMouseButton(0))
            MouseDrag();
        if (Input.GetMouseButtonUp(0))
            MouseUp();
    }

    void MouseDown()
    {
        // RaycastHit ray;
        // Physics.Raycast(getMouseRay(), out ray, 500, slotLayer);
    }

    void MouseDrag()
    {
        RaycastHit ray;
        Physics.Raycast(getMouseRay(), out ray, 500, slotLayer);
        if (ray.collider == null)
        {
            currentSlot?.Deselected();
            currentSlot = null;
            return;
        }

        if (!slotHolder.ContainsKey(ray.collider))
        {
            slotHolder.Add(ray.collider,ray.collider.GetComponentInParent<Slot>());
        }
        Slot slot = slotHolder[ray.collider];
        
        if (currentSlot != slot)
        {
            currentSlot?.Deselected();
            currentSlot = slot;
            currentSlot?.Selected();
        }
    }

    void MouseUp()
    {
        currentSlot?.Deselected();
        currentSlot = null;
    }


    private Ray getMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
}