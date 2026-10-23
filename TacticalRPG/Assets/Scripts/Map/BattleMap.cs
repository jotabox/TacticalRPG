using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace TacticalRPG.Map
{
    public class BattleMap 
    {
        private readonly Cell[,] _cells;

        public int Width { get; }
        public int Height { get; }

        public BattleMap(TerrainKind[,] terrains)
        {
            Width = terrains.GetLength(0);
            Height = terrains.GetLength(1);
            _cells = new Cell[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    _cells[x, y] = new Cell(new Vector2Int(x, y), terrains[x, y]);
                }
            }
        }

        public bool IsInside(Vector2Int position)
        {
            return position.x >= 0 && position.x < Width
                && position.y >= 0 && position.y < Height;
        }

        public Cell GetCell(Vector2Int position)
        {
            return _cells[position.x, position.y];
        }

        private static readonly Vector2Int[] Directions =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        public List<Vector2Int> GetNeighbors(Vector2Int position)
        {
            var neighbors = new List<Vector2Int>(4);
            foreach (Vector2Int direction in Directions)
            {
                Vector2Int candidate = position + direction;
                if (IsInside(candidate))
                {
                    neighbors.Add(candidate);
                }
            }
            return neighbors;
        }

        public static int Distance(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

    }
}
