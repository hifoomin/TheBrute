#region

using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using TheBrute.Relics.Uncommons;

#endregion

namespace TheBrute.Cards
{
    public class MaxHpTracker() : CustomSingletonModel(HookType.Combat)
    {
        public static readonly SpireField<Creature, decimal> totalMaxHpGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> totalMaxHpLostThisCombat = new(() => 0);

        public static readonly SpireField<Creature, decimal> timesMaxHpGainedThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> timesMaxHpLostThisCombat = new(() => 0);
        public static readonly SpireField<Creature, decimal> timesMaxHpLostWithBloodBank = new(() => 0);

        public static readonly SpireField<Creature, bool> gainedMaxHpThisTurn = new(() => false);
        public static readonly SpireField<Creature, bool> lostMaxHpThisTurn = new(() => false);

        public static readonly SpireField<Creature, bool> rozjebKurwaJebanyHealKurwaKurwaGownoKurwaPierdoloneKurwa = new(() => false);

        public static decimal GetTotalMaxHpGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalMaxHpGainedThisCombat[creature];
        }

        public static decimal GetTotalMaxHpLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : totalMaxHpLostThisCombat[creature];
        }

        public static decimal GetTimesMaxHpGainedThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesMaxHpGainedThisCombat[creature];
        }

        public static decimal GetTimesMaxHpLostThisCombat(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState == null ? 0 : timesMaxHpLostThisCombat[creature];
        }

        public static bool GetGainedMaxHpThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && gainedMaxHpThisTurn[creature];
        }

        public static bool GetLostMaxHpThisTurn(Creature creature)
        {
            var combatState = creature.CombatState;
            return combatState != null && lostMaxHpThisTurn[creature];
        }

        public static bool GetChangedMaxHpThisTurn(Creature creature)
        {
            return GetGainedMaxHpThisTurn(creature) || GetLostMaxHpThisTurn(creature);
        }

        public static decimal GetTotalChangedGoldThisCombat(Creature creature)
        {
            return GetTotalMaxHpGainedThisCombat(creature) + Math.Abs(GetTotalMaxHpLostThisCombat(creature));
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    if (player.PlayerCombatState!.TurnNumber == 1)
                    {
                        totalMaxHpGainedThisCombat[player.Creature] = 0;
                        timesMaxHpGainedThisCombat[player.Creature] = 0;

                        totalMaxHpLostThisCombat[player.Creature] = 0;
                        timesMaxHpLostThisCombat[player.Creature] = 0;
                    }
                    gainedMaxHpThisTurn[player.Creature] = false;
                    lostMaxHpThisTurn[player.Creature] = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(CreatureCmd), "GainMaxHp")]
    internal class MaxHpTrackerGainMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount)
        {
            var combatState = creature.CombatState;
            if (combatState != null && creature.IsPlayer)
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

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class MaxHpTrackerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;
            if (combatState != null && creature.IsPlayer)
            {
                MaxHpTracker.totalMaxHpLostThisCombat[creature] += amount;
                MaxHpTracker.timesMaxHpLostThisCombat[creature] += 1;
                if (combatState.CurrentSide == CombatSide.Player)
                {
                    MaxHpTracker.lostMaxHpThisTurn[creature] = true;
                }
                // "I woke up 10 minutes ago" code below:
                if (creature.Player?.GetRelic<BloodBank>() != null)
                {
                    var bloodBank = creature.Player.GetRelic<BloodBank>();
                    MaxHpTracker.timesMaxHpLostWithBloodBank[creature] += 1;
                    bloodBank!.CurrentCounter = (int)MaxHpTracker.timesMaxHpLostWithBloodBank[creature];
                    if (MaxHpTracker.timesMaxHpLostWithBloodBank[creature] % bloodBank.DynamicVars.MaxHp.BaseValue == 0)
                    {
                        bloodBank.Flash();
                        CardPileCmd.Draw(new BlockingPlayerChoiceContext(), bloodBank.DynamicVars.Cards.BaseValue, creature.Player);
                        MaxHpTracker.timesMaxHpLostWithBloodBank[creature] = 0;
                        bloodBank.CurrentCounter = (int)MaxHpTracker.timesMaxHpLostWithBloodBank[creature];
                    }
                }
                MaxHpTracker.rozjebKurwaJebanyHealKurwaKurwaGownoKurwaPierdoloneKurwa[creature] = false;
            }
        }
    }
}