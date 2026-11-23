using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

namespace TacticalRPG.View
{
    public class MapView : MonoBehaviour
    {
        [SerializeField] private Tilemap highlightTilemap;
        [SerializeField] private TileBase moveHighlightTile;


        public void ShowReachable(IEnumerable<Vector2Int> cells)
        {
            highlightTilemap.ClearAllTiles();
            foreach (Vector2Int cell in cells)
            {
                highlightTilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), moveHighlightTile);
            }
        }

        public void ClearHighlight()
        {
            highlightTilemap.ClearAllTiles();
        }

        public Vector3 CellToWorldCenter(Vector2Int cell)
        {
            return highlightTilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
        }

        public Vector2Int WorldToCell(Vector3 worldPosition)
        {
            Vector3Int cell = highlightTilemap.WorldToCell(worldPosition);
            return new Vector2Int(cell.x, cell.y);
        }
    }
}