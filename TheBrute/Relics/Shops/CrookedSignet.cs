#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;

#endregion

namespace TheBrute.Relics.Shops
{
#pragma warning disable STS001 // Symbol missing localization

    internal class CrookedSignet : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        private bool _strengthApplied;
        public override RelicRarity Rarity => RelicRarity.Shop;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new("GoldThreshold", 30m),
            new PowerVar<StrengthPower>(5m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<StrengthPower>()
        ];

        private bool StrengthApplied
        {
            get => _strengthApplied;
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

        public async Task ModifyStrengthIfNecessary()
        {
            var doesntPassThreshold = Owner.Gold > DynamicVars["GoldThreshold"].BaseValue;

            Status = !doesntPassThreshold ? RelicStatus.Active : RelicStatus.Normal;

            var strengthAmount = DynamicVars.Strength.BaseValue;

            if (doesntPassThreshold && StrengthApplied)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, -strengthAmount, Owner.Creature, null);
                StrengthApplied = false;
            }
            else if (!doesntPassThreshold && !StrengthApplied)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, strengthAmount, Owner.Creature, null);
                StrengthApplied = true;
            }
        }

        public override async Task AfterGoldGained(Player player)
        {
            if (player != Owner)
            {
                return;
            }
            if (CombatManager.Instance.IsInProgress)
            {
                await ModifyStrengthIfNecessary();
            }
        }
    }

    /*
        [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "GainGold")]
        internal class CrookedSignetGainGoldPatch
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
        */

    [HarmonyPatch(typeof(PlayerCmd), "LoseGold")]
    internal class CrookedSignetLoseGoldPatch
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