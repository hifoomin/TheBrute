#region

using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

// using TheBrute.Relics.Commons;

namespace TheBrute.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsUpPower : TheBrutePower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            // Main.Logger.Warn("after side turn end called");
            if (participants.Contains(Owner))
            {
                var thornsPower = Owner.GetPower<ThornsPower>();
                if (thornsPower != null && thornsPower.Amount > 0)
                {
                    await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), thornsPower, -Amount, null, null);
                }

                // Main.Logger.Warn("removing TEMP thorns power");
                await PowerCmd.Remove(this);
            }
        }
    }

    [HarmonyPatch(typeof(PowerModel), "SetAmount")]
    public class TemporaryThornsUpPowerJustInCasePatch
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