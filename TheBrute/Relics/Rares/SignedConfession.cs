#region

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Runs;

#endregion

namespace TheBrute.Relics.Rares
{
    internal class SignedConfession : TheBruteRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override Task AfterObtained()
        {
            var runState = Owner.RunState;

            if (runState?.Map == null)
            {
                return Task.CompletedTask;
            }

            var count = runState.Players.Count(p => p.GetRelic<SignedConfession>() != null);

            runState.Map = new SignedConfessionMap(count, runState.Map);

            NMapScreen.Instance?.SetMap(runState.Map, runState.Rng.Seed, false);

            Flash();

            return Task.CompletedTask;
        }

        public override ActMap ModifyGeneratedMap(IRunState runState, ActMap map, int actIndex)
        {
            var signedConfessionCount = runState.Players.Count(player => player.GetRelic<SignedConfession>() != null);
            if (signedConfessionCount <= 0)
            {
                return map;
            }

            Flash();

            return new SignedConfessionMap(signedConfessionCount, map);
        }
    }

    internal class SignedConfessionMap : ActMap
    {
        private readonly MapPoint? _secondBoss;

        public SignedConfessionMap(int signedConfessionCount, ActMap original)
        {
            var oldRows = original.GetRowCount();
            Grid = new MapPoint?[7, oldRows + signedConfessionCount];

            for (var row = 1; row < oldRows; row++)
            {
                for (var column = 0; column < 7; column++)
                {
                    var point = original.GetPoint(column, row);

                    if (point == null)
                    {
                        continue;
                    }

                    Grid[column, row] = point;
                }
            }

            StartingMapPoint = original.StartingMapPoint;

            BossMapPoint = original.BossMapPoint;
            BossMapPoint.coord.row += signedConfessionCount;

            _secondBoss = original.SecondBossMapPoint;

            if (_secondBoss != null)
            {
                _secondBoss.coord.row += signedConfessionCount;
            }

            var parents = BossMapPoint.parents.ToList();

            foreach (var parent in parents)
            {
                parent.RemoveChildPoint(BossMapPoint);

                var previous = parent;

                for (var i = 0; i < signedConfessionCount; i++)
                {
                    var shop = new MapPoint(parent.coord.col, parent.coord.row + i + 1)
                    {
                        PointType = MapPointType.Shop,
                        CanBeModified = false
                    };

                    Grid[shop.coord.col, shop.coord.row] = shop;

                    previous.AddChildPoint(shop);

                    previous = shop;
                }

                previous.AddChildPoint(BossMapPoint);
            }
        }

        public override MapPoint? SecondBossMapPoint => _secondBoss;
        public override MapPoint BossMapPoint { get; }
        public override MapPoint StartingMapPoint { get; }
        protected override MapPoint?[,] Grid { get; }
    }
}