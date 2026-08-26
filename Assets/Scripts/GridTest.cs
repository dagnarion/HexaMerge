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
    }
}
