#region

using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace TheBrute.Relics.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class BloodBank : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        private int currentCounter;
        public override bool ShowCounter => true;

        public int CurrentCounter
        {
            get => currentCounter;
            set
            {
                if (currentCounter == value)
                {
                    return;
                }

                currentCounter = value;
                InvokeDisplayAmountChanged();
            }
        }

        public override int DisplayAmount => CurrentCounter;

        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(5m),
            new CardsVar(1)
        ];

        // logic is handled in MaxHpTrackerLoseMaxHpPatch
    }
}