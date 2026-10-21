using UnityEngine;

namespace TacticalRPG.Map
{
    public class Cell 
    {
        public Vector2Int Position { get; }
        public TerrainKind BaseTerrain { get; }

        public Cell (Vector2Int position , TerrainKind baseTerrain)
        {
            Position = position;
            BaseTerrain = baseTerrain;
        }
        
    }
}
