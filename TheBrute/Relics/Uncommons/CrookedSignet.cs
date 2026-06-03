using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;
using TheBrute.Relics;

namespace TheBrute.Relics.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class CrookedSignet : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("GoldThreshold", 30m),
            new PowerVar<StrengthPower>(5m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<StrengthPower>()
        ];

        private bool _strengthApplied;

        private bool StrengthApplied
        {
            get
            {
                return _strengthApplied;
            }
            set
            {
                AssertMutable();
                _strengthApplied = value;
            }
        }

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            if (room is CombatRoom)
            {
                await ModifyStrengthIfNecessary();
            }
        }

        public override Task AfterCombatEnd(CombatRoom _)
        {
            StrengthApplied = false;
            Status = RelicStatus.Normal;
            return Task.CompletedTask;
        }

        private async Task ModifyStrengthIfNecessary()
        {
            var passesThreshold = Owner.Gold > DynamicVars["GoldThreshold"].BaseValue;

            Status = (!passesThreshold) ? RelicStatus.Active : RelicStatus.Normal;

            var strengthAmount = DynamicVars.Strength.BaseValue;

            if (passesThreshold && StrengthApplied)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, -strengthAmount, Owner.Creature, null);
                StrengthApplied = false;
            }
            else if (!passesThreshold && !StrengthApplied)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, strengthAmount, Owner.Creature, null);
                StrengthApplied = true;
            }
        }

        [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "GainGold")]
        internal class GainGoldPatch
        {
            private static void Postfix(Task __result, Player player)
            {
                _ = PostfixAsync(player);
            }

            private static async Task PostfixAsync(Player player)
            {
                var crookedSignet = player.GetRelic<CrookedSignet>();
                if (crookedSignet != null && CombatManager.Instance.IsInProgress)
                {
                    await crookedSignet.ModifyStrengthIfNecessary();
                }
            }
        }

        [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "LoseGold")]
        internal class LoseGoldPatch
        {
            private static void Postfix(Task __result, Player player)
            {
                _ = PostfixAsync(player);
            }

            private static async Task PostfixAsync(Player player)
            {
                var crookedSignet = player.GetRelic<CrookedSignet>();
                if (crookedSignet != null && CombatManager.Instance.IsInProgress)
                {
                    await crookedSignet.ModifyStrengthIfNecessary();
                }
            }
        }
    }
}