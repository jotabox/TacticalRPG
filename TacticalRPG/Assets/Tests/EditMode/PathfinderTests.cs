using NUnit.Framework;
using System;
using TacticalRPG.Map;
using TTacticalRPG.Map;
using UnityEngine;

namespace TacticalRPG.Tests
{
    public class PathfinderTests
    {
        // A primeira  linha  é o topo do mapa (maior Y)
        private static BattleMap MapFrom(params string[] rows)
        {
            int height = rows.Length;
            int width = rows[0].Length;
            var terrains = new TerrainKind[width, height];

            for (int row = 0; row < height; row++)
            {
                int y = height - 1 - row;
                for (int x = 0; x < width; x++)
                {
                    terrains[x, y] = rows[row][x] switch
                    {
                        'P' => TerrainKind.Plains,
                        'F' => TerrainKind.Forest,
                        'W' => TerrainKind.Water,
                        _ => throw new ArgumentException($"Letra Inválida: {rows[row][x]}")
                    };
                }
            }

            return new BattleMap(terrains);
        }

        [Test]
        public void OpenPlains_ReachesEveryCellWithinMovement()
        {
            BattleMap map = MapFrom("PPPPP", "PPPPP", "PPPPP", "PPPPP", "PPPPP");

            var reachable = Pathfinder.GetReachableCells(map, new Vector2Int(2, 2), 2);

            Assert.AreEqual(13, reachable.Count);
            Assert.AreEqual(0, reachable[new Vector2Int(2, 2)]);
            Assert.AreEqual(2, reachable[new Vector2Int(4, 2)]);
        }

        [Test]
        public void Forest_CostsTwo()
        {
            BattleMap map = MapFrom("PFP");

            var reachable = Pathfinder.GetReachableCells(map, new Vector2Int(0, 0), 2);

            Assert.AreEqual(2, reachable[new Vector2Int(1, 0)]);
            Assert.IsFalse(reachable.ContainsKey(new Vector2Int(2, 0)));
        }

        [Test]
        public void Water_BlocksEvenWithLargeMovement()
        {
            BattleMap map = MapFrom("PWP");

            var reachable = Pathfinder.GetReachableCells(map, new Vector2Int(0, 0), 10);

            Assert.AreEqual(1, reachable.Count);
        }

        [Test]
        public void ChoosesCheapestPath_NotTheShortestInSteps()
        {
            // Direto pelo meio: 2+2+2+1 = 7. Contornando por baixo: 6.
            BattleMap map = MapFrom("PFFFP", "PPPPP");

            var reachable = Pathfinder.GetReachableCells(map, new Vector2Int(0, 1), 10);

            Assert.AreEqual(6, reachable[new Vector2Int(4, 1)]);
        }

        [Test]
        public void ZeroMovement_ReachesOnlyTheStartCell()
        {
            BattleMap map = MapFrom("PPP");

            var reachable = Pathfinder.GetReachableCells(map, new Vector2Int(1, 0), 0);

            Assert.AreEqual(1, reachable.Count);
            Assert.AreEqual(0, reachable[new Vector2Int(1, 0)]);
        }

        [Test]
        public void StartOutsideTheMap_Throws()
        {
            BattleMap map = MapFrom("PPP");

            Assert.Throws<ArgumentOutOfRangeException>(
                () => Pathfinder.GetReachableCells(map, new Vector2Int(-1, 0), 3));
        }

    }
}
