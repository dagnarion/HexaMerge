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

    private void Merge(Vector3Int currentPos, CellStack cellStack)
    {
        List<CellStack> stacks = GetStacksCanMerge(currentPos, cellStack);
    }

    private List<CellStack> GetStacksCanMerge(Vector3Int currentPos, CellStack cellStack)
    {
        List<CellStack> stackHolder = new List<CellStack>();
        if (!grid.IsOnGrid(currentPos)) return stackHolder;
        Slot startSlot = grid.GetValue(currentPos);
        if (startSlot == null || startSlot.CellStack == null || startSlot.CellStack.IsEmpty()) return stackHolder;
        FloodFill(currentPos, cellStack, stackHolder);
        return stackHolder;
    }

    private void FloodFill(Vector3Int currentPosition, CellStack oldStalk, List<CellStack> stackHolder)
    {
        CellStack currentCellStack = gridController.Grid.GetValue(currentPosition).CellStack;
        if (currentCellStack.GetTopCell().color != oldStalk.GetTopCell().color) return;
        if (stackHolder.Contains(currentCellStack)) return;
        stackHolder.Add(currentCellStack);
        foreach (Vector2Int direc in Direction.Direct)
        {
            Vector3Int nextPosition = currentPosition + (Vector3Int)direc;
            Slot nextSlot = grid.GetValue(nextPosition);
            if (!grid.IsOnGrid(nextPosition)
                || nextSlot == null
                || nextSlot.CellStack == null
                || nextSlot.CellStack.IsEmpty()) continue;

            FloodFill(nextPosition, currentCellStack, stackHolder);
        }
    }
}