using System.Collections.Generic;
using UnityEngine;
using System;
using TacticalRPG.Map;

namespace TTacticalRPG.Map
{
    public static class Pathfinder
    {
        public static Dictionary<Vector2Int, int> GetReachableCells(
            BattleMap map, Vector2Int start, int movement)
        {
            if (!map.IsInside(start))
            {
                throw new ArgumentOutOfRangeException(nameof(start), start, "A origem está fora do mapa");
            }
            var costs = new Dictionary<Vector2Int, int> { { start, 0 } };
            var frontier = new List<Vector2Int> { start };

            while (frontier.Count > 0)
            {
                Vector2Int current = TakeCheapest(frontier, costs);

                foreach (Vector2Int neighbor in map.GetNeighbors(current))
                {
                    int? stepCost = TerrainCosts.GetWalkingCost(map.GetCell(neighbor).BaseTerrain);
                    if (stepCost == null)
                    {
                        continue;
                    }

                    int newCost = costs[current] + stepCost.Value;
                    if (newCost > movement)
                    {
                        continue;
                    }

                    if (costs.TryGetValue(neighbor, out int KnownCost) && KnownCost <= newCost)
                    {
                        continue;
                    }

                    costs[neighbor] = newCost;
                    if (!frontier.Contains(neighbor))
                    {
                        frontier.Add(neighbor);
                    }
                }

            }

            return costs;
        }

        private static Vector2Int TakeCheapest(List<Vector2Int> frontier, Dictionary<Vector2Int, int> costs)
        {
            int bestIndex = 0;
            for(int i =1; i < frontier.Count; i++)
            {
                if (costs[frontier[i]] < costs[frontier[bestIndex]])
                {
                    bestIndex = 1;
                }
            }

            Vector2Int best = frontier[bestIndex];
            frontier.RemoveAt(bestIndex);
            return best;
        }

    }
}
