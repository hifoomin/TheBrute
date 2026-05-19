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
using Yoka.Relics.Commons;

namespace Yoka.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsPower : YokaPower
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

        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side == CombatSide.Enemy)
            {
                if (Owner.Player != null && Owner.Player.GetRelic<ThornyHelmet>() != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        await PowerCmd.Decrement(this);
                    }

                    return;
                }
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