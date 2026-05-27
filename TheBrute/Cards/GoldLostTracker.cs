using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;
using TheBrute.Relics.Uncommons;

namespace TheBrute.Cards
{
    public class GoldLostTracker() : CustomSingletonModel(true, false)
    {
        public static readonly SpireField<ICombatState, decimal> totalGoldGainedThisCombat = new(() => 0);
        public static readonly SpireField<ICombatState, decimal> totalGoldLostThisCombat = new(() => 0);

        public static readonly SpireField<ICombatState, decimal> timesGoldGainedThisCombat = new(() => 0);
        public static readonly SpireField<ICombatState, decimal> timesGoldLostThisCombat = new(() => 0);

        public static readonly SpireField<ICombatState, bool> gainedGoldThisTurn = new(() => false);
        public static readonly SpireField<ICombatState, bool> lostGoldThisTurn = new(() => false);

        public static decimal GetTotalGoldGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalGoldGainedThisCombat[combatState];
        }

        public static decimal GetTotalGoldLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalGoldLostThisCombat[combatState];
        }

        public static decimal GetTimesGoldGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesGoldGainedThisCombat[combatState];
        }

        public static decimal GetTimesGoldLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesGoldLostThisCombat[combatState];
        }

        public static bool GetGainedGoldThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && gainedGoldThisTurn[combatState];
        }

        public static bool GetLostGoldThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostGoldThisTurn[combatState];
        }

        public static bool GetChangedGoldThisTurn(Creature creature)
        {
            return GetLostGoldThisTurn(creature) || GetGainedGoldThisTurn(creature);
        }

        public static decimal GetTotalChangedGoldThisCombat(Creature creature)
        {
            return GetTotalGoldGainedThisCombat(creature) + Math.Abs(GetTotalGoldLostThisCombat(creature));
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    GoldLostTracker.gainedGoldThisTurn[combatState] = false;
                    GoldLostTracker.lostGoldThisTurn[combatState] = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "GainGold")]
    internal class GainGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                GoldLostTracker.totalGoldGainedThisCombat[combatState] += amount;
                GoldLostTracker.timesGoldGainedThisCombat[combatState] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    GoldLostTracker.gainedGoldThisTurn[combatState] = true;
                }
            }
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
                GoldLostTracker.totalGoldLostThisCombat[combatState] += amount;
                GoldLostTracker.timesGoldLostThisCombat[combatState] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    GoldLostTracker.lostGoldThisTurn[combatState] = true;
                }
            }
        }
    }
}