using System;
using UnityEngine;

namespace TacticalRPG.Map
{
    public static class TerrainCosts
    {
        public static int? GetWalkingCost(TerrainKind terrain)
        {
            switch (terrain)
            {
                case TerrainKind.Plains: return 1;
                case TerrainKind.Forest: return 2;
                case TerrainKind.Water: return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(terrain), terrain, "terreno sem custo definido");
            }
        }


    }
}
