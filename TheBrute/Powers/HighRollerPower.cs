using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;

namespace TheBrute.Powers
{
    internal class HighRollerPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "GainGold")]
    internal class HighRollerPowerGainGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player)
        {
            var combatState = player.Creature.CombatState;
            var highRollerPower = player.Creature.GetPower<HighRollerPower>();
            if (combatState != null && highRollerPower != null)
            {
                highRollerPower.Flash();
                PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, highRollerPower.Amount, player.Creature, null);
            }
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "LoseGold")]
    internal class HighRollerPowerLoseGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player, GoldLossType goldLossType)
        {
            var combatState = player.Creature.CombatState;
            var highRollerPower = player.Creature.GetPower<HighRollerPower>();
            if (combatState != null && highRollerPower != null)
            {
                highRollerPower.Flash();
                PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, highRollerPower.Amount, player.Creature, null);
            }
        }
    }
}