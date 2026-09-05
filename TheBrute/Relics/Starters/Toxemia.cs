#region

using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheBrute.Cards;
using TheBrute.Relics.Ancients;

#endregion

namespace TheBrute.Relics.Starters
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Toxemia : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        private int currentCounter;
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

        public override int DisplayAmount => CurrentCounter;

        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            // new MaxHpVar(2m)
            new CardsVar(1)
        ];

        public override RelicModel GetUpgradeReplacement()
        {
            return ModelDb.Relic<Symbiosis>();
        }

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

        // "THEBRUTE-TOXEMIA.description": "At the end of combat, gain [blue]{MaxHp}[/blue] Max HP.",
        // "THEBRUTE-TOXEMIA.flavor": "The ambivalence in despair.",
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class ToxemiaLoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;
            if (creature.Player == null || !creature.Player.TryGetRelic<Toxemia>(out var toxemia))
            {
                return;
            }

            var normalizedAmount = Math.Abs(amount); // idfk it shouldnt be negative anyway lmao
            if (combatState == null || normalizedAmount <= 0 || toxemia.CurrentCounter <= 0 /* && isFromCard*/)
            {
                return;
            }

            MaxHpTracker.suppressedGainMaxHpHeals[creature]++;
            CreatureCmd.GainMaxHp(creature, normalizedAmount);
            toxemia.CurrentCounter = Math.Max(0, toxemia.CurrentCounter - 1);
            toxemia.InvokeDisplayAmountChanged();
        }
    }

    [HarmonyPatch]
    internal static class GainMaxHpPatch
    {
        private static MethodBase TargetMethod()
        {
            var stateMachine = typeof(CreatureCmd).GetNestedType("<GainMaxHp>d__22", BindingFlags.NonPublic);

            return stateMachine?.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var heal = AccessTools.Method(typeof(CreatureCmd), nameof(CreatureCmd.Heal), [
                typeof(Creature),
                typeof(decimal),
                typeof(bool)
            ]);

            var replacement = AccessTools.Method(typeof(GainMaxHpPatch), nameof(FixFuckingHealing));

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(heal))
                {
                    yield return new CodeInstruction(OpCodes.Call, replacement);
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public static Task FixFuckingHealing(Creature creature, decimal amount, bool playAnim)
        {
            if (MaxHpTracker.suppressedGainMaxHpHeals[creature] > 0)
            {
                MaxHpTracker.suppressedGainMaxHpHeals[creature]--;
                return Task.CompletedTask;
            }

            return CreatureCmd.Heal(creature, amount, playAnim);
        }
    }
}