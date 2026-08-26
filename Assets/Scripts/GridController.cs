using NaughtyAttributes;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private Transform board;
    [SerializeField] private Slot slot;
    [SerializeField] private int gridSize;
    
    [Button]
    public void GenerateGrid()
    {
        board.transform.Clear();
        for(int x = -gridSize;x<=gridSize;x++)
        for (int y = -gridSize; y <= gridSize; y++)
        {
            Vector3 position = grid.CellToWorld(new Vector3Int(x, y, 0));
            Instantiate(slot, position, Quaternion.identity, board);
        }
    }
}
