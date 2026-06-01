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
using MegaCrit.Sts2.Core.Rooms;
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
    public class GoldTracker() : CustomSingletonModel(HookType.Combat)
    {
        public static readonly SpireField<Creature, decimal> totalGoldGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> totalGoldLostThisCombat = new(() => 0);

        public static readonly SpireField<Creature, decimal> timesGoldGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> timesGoldLostThisCombat = new(() => 0);

        public static readonly SpireField<Creature, bool> gainedGoldThisTurn = new(() => false);
        public static readonly SpireField<Creature, bool> lostGoldThisTurn = new(() => false);

        public static decimal GetTotalGoldGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalGoldGainedThisCombat[creature];
        }

        public static decimal GetTotalGoldLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalGoldLostThisCombat[creature];
        }

        public static decimal GetTimesGoldGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesGoldGainedThisCombat[creature];
        }

        public static decimal GetTimesGoldLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesGoldLostThisCombat[creature];
        }

        public static bool GetGainedGoldThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && gainedGoldThisTurn[creature];
        }

        public static bool GetLostGoldThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostGoldThisTurn[creature];
        }

        public static bool GetChangedGoldThisTurn(Creature creature)
        {
            return GetLostGoldThisTurn(creature) || GetGainedGoldThisTurn(creature);
        }

        public static decimal GetTotalChangedGoldThisCombat(Creature creature)
        {
            return GetTotalGoldGainedThisCombat(creature) + Math.Abs(GetTotalGoldLostThisCombat(creature));
        }

        /*
        public override async Task AfterCombatEnd(CombatRoom room)
        {
            foreach (Creature creature in room.Allies)
            {
                Main.Logger.Warn("after combat end resetting all gold tracker values for " + creature.Name);
                totalGoldGainedThisCombat[creature] = 0;
                totalGoldLostThisCombat[creature] = 0;

                timesGoldGainedThisCombat[creature] = 0;
                timesGoldLostThisCombat[creature] = 0;

                gainedGoldThisTurn[creature] = false;
                lostGoldThisTurn[creature] = false;
            }
        }
        */

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    if (player.PlayerCombatState.TurnNumber == 1)
                    {
                        GoldTracker.totalGoldGainedThisCombat[player.Creature] = 0;
                        GoldTracker.timesGoldGainedThisCombat[player.Creature] = 0;

                        GoldTracker.totalGoldLostThisCombat[player.Creature] = 0;
                        GoldTracker.timesGoldLostThisCombat[player.Creature] = 0;
                    }
                    GoldTracker.gainedGoldThisTurn[player.Creature] = false;
                    GoldTracker.lostGoldThisTurn[player.Creature] = false;
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
            if (combatState != null && combatState.IsLiveCombat())
            {
                GoldTracker.totalGoldGainedThisCombat[player.Creature] += amount;
                GoldTracker.timesGoldGainedThisCombat[player.Creature] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    GoldTracker.gainedGoldThisTurn[player.Creature] = true;
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
            if (combatState != null && combatState.IsLiveCombat())
            {
                GoldTracker.totalGoldLostThisCombat[player.Creature] += amount;
                GoldTracker.timesGoldLostThisCombat[player.Creature] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    GoldTracker.lostGoldThisTurn[player.Creature] = true;
                }
            }
        }
    }
}