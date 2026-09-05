#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.RelicPools;

#endregion

namespace TheBrute.Relics.Trash
{
    [Pool(typeof(EventRelicPool))]
    internal class ArcheArmor : TheBruteRelic
    {
        public override RelicRarity Rarity => RelicRarity.Event;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PlatingPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<PlatingPower>(1),
            new("KurwaTrashHeap", 0m)
        ];

        public override decimal ModifyPowerAmountGivenAdditive(PowerModel power, Creature giver, decimal amount, Creature? target, CardModel? cardSource)
        {
            if (power is not PlatingPower)
            {
                return 0m;
            }
            if (target != Owner.Creature)
            {
                return 0m;
            }

            Flash();

            return DynamicVars["PlatingPower"].BaseValue;
        }
    }
}