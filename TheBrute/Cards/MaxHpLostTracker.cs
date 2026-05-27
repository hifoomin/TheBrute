using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
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
using TheBrute.Relics.Uncommons;

namespace TheBrute.Cards
{
    public class MaxHpLostTracker() : CustomSingletonModel(true, false)
    {
        public static readonly SpireField<ICombatState, decimal> totalMaxHpLostFromCardsThisCombat = new(() => 0);
        public static readonly SpireField<ICombatState, decimal> timesMaxHpLostFromCardsThisCombat = new(() => 0);
        public static readonly SpireField<ICombatState, bool> lostMaxHpFromCardThisTurn = new(() => false);

        public static decimal GetTotalMaxHpLostFromCardsThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalMaxHpLostFromCardsThisCombat[combatState];
        }

        public static decimal GetTimesMaxHpLostFromCardsThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesMaxHpLostFromCardsThisCombat[combatState];
        }

        public static bool GetLostMaxHpFromCardThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostMaxHpFromCardThisTurn[combatState];
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    MaxHpLostTracker.lostMaxHpFromCardThisTurn[combatState] = false;
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
                MaxHpLostTracker.totalMaxHpLostFromCardsThisCombat[combatState] += amount;
                MaxHpLostTracker.timesMaxHpLostFromCardsThisCombat[combatState] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    MaxHpLostTracker.lostMaxHpFromCardThisTurn[combatState] = true;
                }
            }
        }
    }
}