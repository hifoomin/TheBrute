#region

using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

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

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    if (player.PlayerCombatState.TurnNumber == 1)
                    {
                        totalGoldGainedThisCombat[player.Creature] = 0;
                        timesGoldGainedThisCombat[player.Creature] = 0;

                        totalGoldLostThisCombat[player.Creature] = 0;
                        timesGoldLostThisCombat[player.Creature] = 0;
                    }
                    gainedGoldThisTurn[player.Creature] = false;
                    lostGoldThisTurn[player.Creature] = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(PlayerCmd), "GainGold")]
    internal class GoldTrackerGainGoldPatch
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

    [HarmonyPatch(typeof(PlayerCmd), "LoseGold")]
    internal class GoldTrackerLoseGoldPatch
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