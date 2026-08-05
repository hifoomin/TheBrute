using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Hooks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using TheBrute.Cards;

namespace TheBrute.Powers
{
    internal class UnravelPower : TheBrutePower, IHasSecondAmount
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private int _usesLeft = 0;

        public int UsesLeft
        {
            get
            {
                return _usesLeft;
            }
            set
            {
                AssertMutable();
                _usesLeft = value;
            }
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (applier == Owner && power == this)
            {
                UsesLeft = Amount;
                this.InvokeSecondAmountChanged();
            }
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner))
            {
                UsesLeft = Amount;
                this.InvokeSecondAmountChanged();
            }
        }

        public string GetSecondAmount()
        {
            return $"{UsesLeft}";
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CreatureCmd), "LoseMaxHp")]
    internal class UnravelPowerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;
            var unravelPower = creature.GetPower<UnravelPower>();
            var normalizedAmount = Math.Abs(amount); // idfk it shouldnt be negative anyway lmao
            if (combatState != null && unravelPower != null && normalizedAmount > 0 && unravelPower.UsesLeft > 0 /* && isFromCard*/)
            {
                MaxHpTracker.rozjebKurwaJebanyHealKurwaKurwaGownoKurwaPierdoloneKurwa[creature] = true;
                CreatureCmd.GainMaxHp(creature, normalizedAmount);
                unravelPower.UsesLeft--;
                unravelPower.InvokeSecondAmountChanged();
                MaxHpTracker.rozjebKurwaJebanyHealKurwaKurwaGownoKurwaPierdoloneKurwa[creature] = false;
            }
        }
    }

    [HarmonyPatch]
    public class GainMaxHpPatch
    {
        private static MethodBase TargetMethod()
        {
            var stateMachine = typeof(CreatureCmd).GetNestedType("<GainMaxHp>d__22", BindingFlags.NonPublic);

            return stateMachine.GetMethod("MoveNext", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var szmataKurwaPierdolanaDziwka = AccessTools.Method(typeof(CreatureCmd), nameof(CreatureCmd.Heal), [typeof(Creature), typeof(decimal), typeof(bool)]);

            var kurwaJebana = AccessTools.Method(typeof(GainMaxHpPatch), nameof(KurwaGownoJebaneSpierdoloneKurwaSzmatyJebaneKurwaBezmozgieKurwyKurwa));

            foreach (var instruction in instructions)
            {
                if (instruction.Calls(szmataKurwaPierdolanaDziwka))
                {
                    yield return new CodeInstruction(OpCodes.Call, kurwaJebana);
                }
                else
                {
                    yield return instruction;
                }
            }
        }

        public static Task KurwaGownoJebaneSpierdoloneKurwaSzmatyJebaneKurwaBezmozgieKurwyKurwa(Creature creature, decimal amount, bool playAnim)
        {
            if (MaxHpTracker.rozjebKurwaJebanyHealKurwaKurwaGownoKurwaPierdoloneKurwa[creature])
            {
                return Task.CompletedTask;
            }

            return CreatureCmd.Heal(creature, amount, playAnim);
        }
    }
}