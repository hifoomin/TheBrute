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
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            if (combatState != null && unravelPower != null && unravelPower.Amount > 0 && unravelPower.UsesLeft > 0 /* && isFromCard*/)
            {
                CreatureCmd.GainMaxHp(creature, amount);
                unravelPower.UsesLeft--;
                unravelPower.InvokeSecondAmountChanged();
            }
        }
    }
}