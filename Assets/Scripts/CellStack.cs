using System.Collections.Generic;
using UnityEngine;

public class CellStack : MonoBehaviour
{
    private List<Cell> cellHolder = new List<Cell>();

    public Cell GetCell(int index)
    {
        if (index >= cellHolder.Count) return null;
        return cellHolder[index];
    }
    
    public int GetStackSize() => cellHolder.Count;
    
    public void CanSelect()
    {
        foreach (var it in cellHolder)
        {
            it.CanSelect();
        }
    }

    public void UnSelect()
    {
        foreach (var it in cellHolder)
        {
            it.UnSelect();
        }
    }

    public void SetParent(Transform parent)
    {
        transform.SetParent(parent);
    }

    public void Add(Cell anotherCell)
    {
        cellHolder.Add(anotherCell);
    }

    public void Remove(int index)
    {
        if(IsEmpty() || index >= cellHolder.Count) return;
        cellHolder.RemoveAt(index);
    }

    public bool IsEmpty() => cellHolder.Count <= 0;
    
    
}
