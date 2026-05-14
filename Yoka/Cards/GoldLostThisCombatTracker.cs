using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics.Uncommons;

namespace Yoka.Cards
{
    public class GoldLostThisCombatTracker() : CustomSingletonModel(true, false)
    {
        public static readonly SpireField<ICombatState, decimal> GoldLost = new(() => 0);

        public static decimal Get(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : GoldLost[combatState];
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "LoseGold")]
    internal class LoseGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player, GoldLossType goldLossType)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                GoldLostThisCombatTracker.GoldLost[combatState] += amount;
            }
        }
    }
}