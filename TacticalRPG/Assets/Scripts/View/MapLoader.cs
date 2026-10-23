using System;
using System.Collections.Generic;
using TacticalRPG.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TacticalRPG.View
{
    public class MapLoader : MonoBehaviour
    {
        [SerializeField] private Tilemap terrainTilemap;
        [SerializeField] private TileBase plainsTile;
        [SerializeField] private TileBase forestTile;
        [SerializeField] private TileBase waterTile;

        public BattleMap Map { get; private set; }

        private void Awake()
        {
            Map = Load();
            Debug.Log($"Mapa Carregado : {Map.Width} x {Map.Height}."
                + $"Célula (0,0): {Map.GetCell(Vector2Int.zero).BaseTerrain}");
            Debug.Log($"Vizinhos de (0,0): {Map.GetNeighbors(new Vector2Int(0, 0)).Count} (esperado 2)");
            Debug.Log($"Vizinhos de (5,5): {Map.GetNeighbors(new Vector2Int(5, 5)).Count} (esperado 4)");
            Debug.Log($"Distância (0,0)-(3,4): {BattleMap.Distance(new Vector2Int(0, 0), new Vector2Int(3, 4))} (esperado 7)");
        }

        private BattleMap Load()
        {
            var tileToTerrain = new Dictionary<TileBase, TerrainKind>
            {
                {plainsTile, TerrainKind.Plains },
                {forestTile, TerrainKind.Forest },
                {waterTile, TerrainKind.Water }
            };

            terrainTilemap.CompressBounds();
            BoundsInt bounds = terrainTilemap.cellBounds;

            if(bounds.xMin != 0 || bounds.yMin != 0)
            {
                Debug.LogWarning("O mapa deveria começar na célula (0,0).");
            }

            int width = bounds.size.x;
            int height = bounds.size.y;
            var terrains = new TerrainKind[width, height];
            int invalidCells = 0;

            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    TileBase tile = terrainTilemap.GetTile(new Vector3Int(x, y, 0));

                    if(tile == null)
                    {
                        Debug.LogError($"Célula ({x},{y}) está vazia");
                        invalidCells++;
                        continue;
                    }

                    if(!tileToTerrain.TryGetValue(tile, out TerrainKind kind))
                    {
                        Debug.LogError($"Célula ({x},{y}) tem um tile desconhecido: {tile.name}");
                        invalidCells++;
                        continue;
                    }

                    terrains[x, y] = kind;
                }
            }

            if (invalidCells > 0)
            {
                throw new InvalidOperationException(
                    $"O mapa tem {invalidCells} célula(s) inválida(s).");
            }

            return new BattleMap(terrains);
        }
    }
}
