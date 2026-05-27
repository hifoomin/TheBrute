/*
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using TheBrute.Relics.Commons;

namespace TheBrute.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsPower : TheBrutePower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeDamageReceived(PlayerChoiceContext choiceContext, Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (target == Owner && dealer != null && (props.IsPoweredAttack() || cardSource is Omnislice))
            {
                Flash();
                await CreatureCmd.Damage(choiceContext, dealer, Amount, ValueProp.Unpowered | ValueProp.SkipHurtAnim, Owner, null);
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (side == CombatSide.Enemy)
            {
                await PowerCmd.Remove(this);
            }
        }
    }

    [HarmonyPatch(typeof(PowerModel), "SetAmount")]
    public class KurwaMacGownoZjebanePatchxDD
    {
        private static void Prefix(PowerModel __instance, ref int amount)
        {
            if (__instance is TemporaryThornsPower)
            {
                amount = Mathf.Max(amount, 0);
            }
        }
    }
}
*/