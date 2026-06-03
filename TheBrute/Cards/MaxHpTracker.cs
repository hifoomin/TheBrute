using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
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
using TheBrute.Relics.Uncommons;

namespace TheBrute.Cards
{
    public class MaxHpTracker() : CustomSingletonModel(true, false)
    {
        public static readonly SpireField<Creature, decimal> totalMaxHpGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> totalMaxHpLostFromCardsThisCombat = new(() => 0);

        public static readonly SpireField<Creature, decimal> timesMaxHpGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> timesMaxHpLostFromCardsThisCombat = new(() => 0);

        public static readonly SpireField<Creature, bool> gainedMaxHpThisTurn = new(() => false);
        public static readonly SpireField<Creature, bool> lostMaxHpFromCardThisTurn = new(() => false);

        public static decimal GetTotalMaxHpGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalMaxHpGainedThisCombat[creature];
        }

        public static decimal GetTotalMaxHpLostFromCardsThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalMaxHpLostFromCardsThisCombat[creature];
        }

        public static decimal GetTimesMaxHpGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesMaxHpGainedThisCombat[creature];
        }

        public static decimal GetTimesMaxHpLostFromCardsThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesMaxHpLostFromCardsThisCombat[creature];
        }

        public static bool GetGainedMaxHpThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostMaxHpFromCardThisTurn[creature];
        }

        public static bool GetLostMaxHpFromCardThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostMaxHpFromCardThisTurn[creature];
        }

        public static bool GetChangedMaxHpThisTurn(Creature creature)
        {
            return GetGainedMaxHpThisTurn(creature) || GetLostMaxHpFromCardThisTurn(creature);
        }

        public static decimal GetTotalChangedGoldThisCombat(Creature creature)
        {
            return GetTotalMaxHpGainedThisCombat(creature) + Math.Abs(GetTotalMaxHpLostFromCardsThisCombat(creature));
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
                        MaxHpTracker.totalMaxHpGainedThisCombat[player.Creature] = 0;
                        MaxHpTracker.timesMaxHpGainedThisCombat[player.Creature] = 0;

                        MaxHpTracker.totalMaxHpLostFromCardsThisCombat[player.Creature] = 0;
                        MaxHpTracker.timesMaxHpLostFromCardsThisCombat[player.Creature] = 0;
                    }
                    MaxHpTracker.gainedMaxHpThisTurn[player.Creature] = false;
                    MaxHpTracker.lostMaxHpFromCardThisTurn[player.Creature] = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CreatureCmd), "GainMaxHp")]
    internal class GainMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount)
        {
            var combatState = creature.CombatState;
            if (combatState != null)
            {
                MaxHpTracker.totalMaxHpGainedThisCombat[creature] += amount;
                MaxHpTracker.timesMaxHpGainedThisCombat[creature] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    MaxHpTracker.gainedMaxHpThisTurn[creature] = true;
                }
            }
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CreatureCmd), "LoseMaxHp")]
    internal class LoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;
            if (combatState != null && isFromCard)
            {
                MaxHpTracker.totalMaxHpLostFromCardsThisCombat[creature] += amount;
                MaxHpTracker.timesMaxHpLostFromCardsThisCombat[creature] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    MaxHpTracker.lostMaxHpFromCardThisTurn[creature] = true;
                }
            }
        }
    }
}