using UnityEngine;

public static class Direction
{
   public static Vector2Int[] GetDirections(Vector3Int position)
   {
      bool isOddRow = Mathf.Abs(position.y) % 2 == 1;

      if (isOddRow)
      {
         return new Vector2Int[]
         {
            new(-1, 0),
            new( 1, 0),
            new( 0, 1),
            new( 1, 1),
            new( 0,-1),
            new( 1,-1)
         };
      }

      return new Vector2Int[]
      {
         new(-1, 0),
         new( 1, 0),
         new(-1, 1),
         new( 0, 1),
         new(-1,-1),
         new( 0,-1)
      };
   }
}
