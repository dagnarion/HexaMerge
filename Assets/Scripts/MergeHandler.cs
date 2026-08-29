using System;
using System.Collections.Generic;
using UnityEngine;

public class MergeHandler : MonoBehaviour
{
    [SerializeField] private GridController gridController;
    [SerializeField] private DragHandler dragHandler;
    private Grid<Slot> grid => gridController.Grid;

    private void OnEnable()
    {
        dragHandler.OnPlaced += Merge;
    }

    private void OnDisable()
    {
        dragHandler.OnPlaced -= Merge;
    }

    private void Merge(Vector3Int currentPos)
    {
        List<Slot> slots = GetSlotCanMerge(currentPos); // tap dinh
        Slot currentSlot = grid.GetValue(currentPos);
        foreach (Slot slot in slots)
        {
            if(slot == currentSlot) continue;
            if(slot.CanPlaced) continue;
            TransferCellBetweenTwoStack(slot.CellStack,currentSlot.CellStack);
            if (slot.CellStack.IsEmpty())
            {
                CellStack cellStack = slot.CellStack;
                slot.SetCellStack(null);
                cellStack.transform.SetParent(null);
                cellStack.gameObject.SetActive(false);
            }   
        }
                
    }

    private void TransferCellBetweenTwoStack(CellStack currentStack,CellStack targetStack)
    {
        Cell topTargetCell = targetStack.GetCell(targetStack.GetStackSize()-1);
        int idx = 1;
        for (int i = currentStack.GetStackSize() - 1; i >= 0; i--)
        {
            if(currentStack.GetCell(i).color != topTargetCell.color) return;
            Cell cell = currentStack.GetCell(i);
            currentStack.Remove(i);
            cell.transform.position = targetStack.GetCell(
                targetStack.GetStackSize() - 1).transform.position.With(y:topTargetCell.transform.position.y + idx++ *.2f);
            cell.transform.SetParent(targetStack.transform);
            targetStack.Add(cell);
        }
    }

    private List<Slot> GetSlotCanMerge(Vector3Int currentPos)
    {
        List<Slot> slotHolder = new List<Slot>();
        FloodFill(currentPos,grid.GetValue(currentPos),slotHolder);
        return slotHolder;
    }

    private void FloodFill(Vector3Int currentPosition, Slot oldSlot, List<Slot> slotHolder)
    {
        Slot currentSlot = grid.GetValue(currentPosition);
        if (currentSlot.CellStack.GetCell(currentSlot.CellStack.GetStackSize() - 1).color !=
            oldSlot.CellStack.GetCell(oldSlot.CellStack.GetStackSize() - 1).color) return;
        slotHolder.Add(currentSlot);
        foreach (Vector2Int direct in Direction.GetDirections(currentPosition))
        {
            Vector3Int nextPosition = currentPosition + (Vector3Int)direct;
            if(!grid.IsOnGrid(nextPosition)) continue;
            Slot nextSlot = grid.GetValue(nextPosition);
            if(nextSlot == null || nextSlot.CanPlaced) continue;
            if(slotHolder.Contains(nextSlot)) continue;
            FloodFill(nextPosition,currentSlot,slotHolder);
        }
    }
    
    
}