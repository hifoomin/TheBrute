#region

using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Cards;

#endregion

namespace TheBrute.Relics.Ancients
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Symbiosis : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        private int currentCounter;
        public override RelicRarity Rarity => RelicRarity.Starter;
        public override bool ShowCounter => CombatManager.Instance.IsInProgress;

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

        // part of this is inside toxemia.cs

        public override int DisplayAmount => CurrentCounter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            // new MaxHpVar(4m)
            new CardsVar(3)
        ];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) /*&& Owner.PlayerCombatState is { TurnNumber: 1 }*/) // waow pattern matching is so cursed lmao, this is apparently both a null check and turn number == 1
            {
                CurrentCounter = DynamicVars.Cards.IntValue;
                InvokeDisplayAmountChanged();
            }
        }

        /*
        public override async Task AfterCombatVictory(CombatRoom _)
        {
            if (!Owner.Creature.IsDead)
            {
                Flash();
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
            }
        }
        */

        // "THEBRUTE-SYMBIOSIS.description": "At the end of combat, gain [blue]{MaxHp}[/blue] Max HP.",
        // "THEBRUTE-SYMBIOSIS.flavor": "A remnant of self-control... trapped, watching all the while...",
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class SymbiosisLoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;

            if (creature.Player == null || !creature.Player.TryGetRelic<Symbiosis>(out var symbiosis))
            {
                return;
            }

            var normalizedAmount = Math.Abs(amount);

            if (combatState == null || normalizedAmount <= 0 || symbiosis.CurrentCounter <= 0)
            {
                return;
            }

            MaxHpTracker.suppressedGainMaxHpHeals[creature]++;
            CreatureCmd.GainMaxHp(creature, normalizedAmount);

            symbiosis.CurrentCounter = Math.Max(0, symbiosis.CurrentCounter - 1);
        }
    }
}