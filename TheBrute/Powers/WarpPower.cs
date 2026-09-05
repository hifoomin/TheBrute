#region

using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class WarpPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Single;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override object InitInternalData()
        {
            return new Data();
        }

        internal bool TryGetCachedThorns(Creature creature, out int amount)
        {
            var result = GetInternalData<Data>().cachedCreatureToThornsAmountMap.TryGetValue(creature, out amount);

            return result;
        }

        private void CacheMaxThorns(Creature creature, int effectiveThorns)
        {
            var dict = GetInternalData<Data>().cachedCreatureToThornsAmountMap;

            if (!dict.TryGetValue(creature, out var cachedThorns))
            {
                dict[creature] = effectiveThorns;
                return;
            }

            var newAmount = Math.Max(cachedThorns, effectiveThorns);
            dict[creature] = newAmount;
        }

        public override Task BeforePowerAmountChanged(PowerModel power, decimal amountToAdd, Creature target, Creature? applier, CardModel? cardSource)
        {
            if (amountToAdd <= 0)
            {
                return Task.CompletedTask;
            }

            if (power is ThornsPower)
            {
                var maxThorns = power.Amount + (int)amountToAdd;
                CacheMaxThorns(target, maxThorns);
            }
            else if (power is TemporaryThornsUpPower)
            {
                var currentThorns = target.GetPower<ThornsPower>()?.Amount ?? 0;
                CacheMaxThorns(target, currentThorns);
            }

            return Task.CompletedTask;
        }

        public override Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power is not ThornsPower thorns)
            {
                return Task.CompletedTask;
            }

            var owner = power.Owner;

            if (owner == null)
            {
                return Task.CompletedTask;
            }

            if (TryGetCachedThorns(owner, out var cachedThorns) && thorns.Amount < cachedThorns)
            {
                thorns.SetAmount(cachedThorns, true);
            }

            return Task.CompletedTask;
        }

        private class Data
        {
            public Dictionary<Creature, int> cachedCreatureToThornsAmountMap = new();
        }
    }

    // this fucking took like 6 or 7 hours cuase I HAET UAJHDJSUZLFJEASDFGHJ(_IKE#WQ(SR)TTDGHGFYJK()EKSRWDFRIPJCGV
}