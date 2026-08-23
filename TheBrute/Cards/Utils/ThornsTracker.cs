#region

using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards
{
    public class ThornsTracker() : CustomSingletonModel(HookType.Combat)
    {
        public static readonly SpireField<Creature, decimal> totalThornsGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> totalThornsLostThisCombat = new(() => 0);

        public static readonly SpireField<Creature, decimal> timesThornsGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> timesThornsLostThisCombat = new(() => 0);

        public static readonly SpireField<Creature, bool> gainedThornsThisTurn = new(() => false);
        public static readonly SpireField<Creature, bool> lostThornsThisTurn = new(() => false);

        public static decimal GetTotalThornsGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalThornsGainedThisCombat[creature];
        }

        public static decimal GetTotalThornsLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalThornsLostThisCombat[creature];
        }

        public static decimal GetTimesThornsGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesThornsGainedThisCombat[creature];
        }

        public static decimal GetTimesThornsLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesThornsLostThisCombat[creature];
        }

        public static bool GetGainedThornsThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && gainedThornsThisTurn[creature];
        }

        public static bool GetLostThornsThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostThornsThisTurn[creature];
        }

        public static bool GetChangedThornsThisTurn(Creature creature)
        {
            return GetGainedThornsThisTurn(creature) || GetLostThornsThisTurn(creature);
        }

        public static decimal GetTotalChangedThornsThisCombat(Creature creature)
        {
            return GetTotalThornsGainedThisCombat(creature) + Math.Abs(GetTotalThornsLostThisCombat(creature));
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
                        totalThornsGainedThisCombat[player.Creature] = 0;
                        timesThornsGainedThisCombat[player.Creature] = 0;

                        totalThornsLostThisCombat[player.Creature] = 0;
                        timesThornsLostThisCombat[player.Creature] = 0;
                    }
                    gainedThornsThisTurn[player.Creature] = false;
                    lostThornsThisTurn[player.Creature] = false;
                }
            }
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            // Main.Logger.Warn("thornstracker: after power amount changed called");
            if (power is not TemporaryThornsUpPower && power is not ThornsPower)
            {
                // Main.Logger.Warn("thornstracker: power is not temporary thorns or not thorns, returning");
                return;
            }

            var creature = power.Owner;
            if (creature == null || creature.CombatState == null || !creature.IsPlayer)
            {
                // Main.Logger.Warn("power owner is null or their combat state is null or its not a player, returning");
                return;
            }

            if (amount > 0)
            {
                // Main.Logger.Warn("temp thorns up or thorns gain is positive, adding to values");
                totalThornsGainedThisCombat[creature] += amount;
                timesThornsGainedThisCombat[creature] += 1;
                if (creature.CombatState!.CurrentSide == CombatSide.Player)
                {
                    gainedThornsThisTurn[creature] = true;
                }
            }
            else
            {
                // Main.Logger.Warn("temp thorns up or thorns gain is NEGATIVE, SUBTRACTING FROMM to values");
                totalThornsLostThisCombat[creature] -= amount;
                timesThornsLostThisCombat[creature] -= 1;
                if (creature.CombatState!.CurrentSide == CombatSide.Player)
                {
                    lostThornsThisTurn[creature] = false;
                }
            }
        }
    }
}