using UnityEngine;
using NUnit.Framework;
using TacticalRPG.Map;

namespace TacticalRPG.Tests
{
    public class BattleMapTests
    {
        private static BattleMap CreateMap(int width, int height)
        {
            return new BattleMap(new TerrainKind[width, height]);
        }

        [Test]
        public void Constructor_UsesWidthAndHeightFromArray()
        {
            BattleMap map = CreateMap(7, 3);

            Assert.AreEqual(7, map.Width);
            Assert.AreEqual(3, map.Height);
        }

        [Test]
        public void Distance_SumsDifferencesOfEachAxis()
        {
            int distance = BattleMap.Distance(new Vector2Int(0, 0), new Vector2Int(3, 4));
            Assert.AreEqual(7, distance);
        }

        [Test]
        public void Distance_IsSymmetric()
        {
            var a = new Vector2Int(1, 5);
            var b = new Vector2Int(4, 2);
            Assert.AreEqual(BattleMap.Distance(a, b), BattleMap.Distance(b, a));
        }

        [Test]
        public void GetNeighbors_AtCorner_ReturnsTwo()
        {
            BattleMap map = CreateMap(7, 3);

            Assert.AreEqual(2, map.GetNeighbors(new Vector2Int(0, 0)).Count);
            Assert.AreEqual(2, map.GetNeighbors(new Vector2Int(6, 2)).Count);
        }

        [Test]
        public void GetNeighbors_InTheMiddle_ReturnsTheFourAdjacentCells()
        {
            BattleMap map = CreateMap(7, 3);

            var expected = new[]
            {
                new Vector2Int(3, 2), new Vector2Int(3, 0),
                new Vector2Int(2, 1), new Vector2Int(4, 1)

            };

            CollectionAssert.AreEquivalent(expected, map.GetNeighbors(new Vector2Int(3, 1)));
        }


    }
}
