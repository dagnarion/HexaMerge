using System.Collections.Generic;
using UnityEngine;

public class CellStack : MonoBehaviour
{
    private List<Cell> cellHolder = new List<Cell>();

    public Cell GetTopCell()
    {
        if (IsEmpty())
        {
            Debug.LogWarning("There were no element in cell");
            return null;
        }

        return cellHolder[^1];
    }

    public Cell GetBottomCell()
    {
        if (IsEmpty())
        {
            Debug.LogWarning("There were no element in cell");
            return null;
        }
        return cellHolder[0];
    }
    
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
