using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using Yoka.Relics.Commons;

namespace Yoka.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsUpPower : YokaPower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            // Main.Logger.Warn("after side turn end called");
            if (participants.Contains(Owner))
            {
                // Main.Logger.Warn("participants contains owner");
                for (int i = 0; i < Amount; i++)
                {
                    // Main.Logger.Warn("decrementing thorns power");
                    await PowerCmd.Decrement(Owner.GetPower<ThornsPower>());
                }
                // Main.Logger.Warn("removing TEMP thorns power");
                await PowerCmd.Remove(this);
            }
        }
    }

    [HarmonyPatch(typeof(PowerModel), "SetAmount")]
    public class JustInCasePatch
    {
        private static void Prefix(PowerModel __instance, ref int amount)
        {
            if (__instance is ThornsPower)
            {
                amount = Mathf.Max(amount, 0);
            }
        }
    }
}