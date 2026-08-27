using System;
using NaughtyAttributes;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private Grid gridRender;
    public Grid<Slot> Grid { get; private set; }
    [SerializeField] private Transform board;
    [SerializeField] private Slot slot;
    [SerializeField] private int gridSize;

    private void Start()
    {
        GenerateGrid();
    }

    [Button]
    public void GenerateGrid()
    {
        board.transform.Clear();
        Grid = new Grid<Slot>(new Vector3Int(gridSize,gridSize), (Vector3Int pos) =>
        {
            Vector3 position = gridRender.CellToWorld(pos);
            return Instantiate(slot, position, Quaternion.identity, board);
        });
    }

    public Vector3Int ConvertWorldPositionToCellPosition(Vector3 position)
    {
        return gridRender.WorldToCell(position);
    }
}
