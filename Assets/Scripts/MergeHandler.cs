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
        dragHandler.OnPlaced += MergeLoop;
    }

    private void OnDisable()
    {
        dragHandler.OnPlaced -= MergeLoop;
    }

    private void MergeLoop(Vector3Int currentPos)
    {
        Queue<Slot> queue = new Queue<Slot>();
        HashSet<Slot> queued = new HashSet<Slot>();
        Slot startSlot = grid.GetValue(currentPos);
        queue.Enqueue(startSlot);
        queued.Add(startSlot);
        while (queue.Count > 0)
        {
            Slot currentSlot = queue.Dequeue();
            queued.Remove(currentSlot);
            List<Slot> changedSlot = Merge(currentSlot);
           bool cleared = ClearStack(currentSlot);
           if(cleared && !currentSlot.CanPlaced) changedSlot.Add(currentSlot); 
            foreach (Slot slot in changedSlot)
            {
                if(slot == null) continue;
                if (queued.Add(slot))
                {
                    queue.Enqueue(slot);
                }
            }
            
        }
    }

    private List<Slot> Merge(Slot startslot)
    {
        List<Slot> slots = GetSlotCanMerge(gridController.ConvertWorldPositionToCellPosition(startslot.transform.position)); // tap dinh
        List<Slot> changedSlot = new List<Slot>();
        Slot currentSlot = startslot;
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
            else changedSlot.Add(slot);
        }

        return changedSlot;
    }

    private bool ClearStack(Slot slot)
    {
        if(slot == null || slot.CanPlaced == null || slot.CellStack.IsEmpty()) return false;
        int cnt = 1;
        CellStack stack = slot.CellStack;
        for (int i = stack.GetStackSize() - 1; i > 0; i--)
        {
            if (stack.GetCell(i).color != stack.GetCell(i - 1).color) break;
            else cnt++;
        }

        if (cnt < 10) return false;
        while (cnt != 0)
        {
            Cell cell = stack.GetCell(stack.GetStackSize()-1);
            stack.Remove(stack.GetStackSize()-1);
            cell.transform.SetParent(null);
            cell.gameObject.SetActive(false);
            cnt--;
        }

        if (stack.IsEmpty())
        {
            slot.SetCellStack(null);
            stack.transform.SetParent(null);
            stack.gameObject.SetActive(false);
        }

        return true;
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
        if (currentSlot.CanPlaced) return;
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