#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Relics.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class ThornyHelmet : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        private bool _usedThisCombat;
        public override RelicRarity Rarity => RelicRarity.Common;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(5m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        private bool UsedThisCombat
        {
            get => _usedThisCombat;
            set
            {
                AssertMutable();
                _usedThisCombat = value;
            }
        }

        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (CombatManager.Instance.IsInProgress && target == Owner.Creature && result.UnblockedDamage > 0 && !UsedThisCombat)
            {
                Flash();
                UsedThisCombat = true;
                await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, null);
            }
        }

        public override Task AfterCombatEnd(CombatRoom _)
        {
            UsedThisCombat = false;
            return Task.CompletedTask;
        }
    }
}