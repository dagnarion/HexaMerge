using NaughtyAttributes;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [OnValueChanged("Setup")]
    [SerializeField] private Vector3Int pos;
    
   public void Setup()
    {
       this.transform.position =  grid.CellToWorld(pos);
       
       Vector3Int center = pos;

       for (int x = -1; x <= 1; x++)
       {
           for (int y = -1; y <= 1; y++)
           {
               if (x == 0 && y == 0) continue;

               Vector3Int pos = center + new Vector3Int(x, y, 0);

               Debug.Log(
                   $"{pos} -> {grid.CellToWorld(pos)}"
               );
           }
       }
    }
}
