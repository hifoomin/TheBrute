#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

namespace TheBrute.Powers
{
    internal class DeathWishPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class DeathWishPowerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, PlayerChoiceContext choiceContext, Creature creature)
        {
            var deathWishPower = creature.GetPower<DeathWishPower>();
            var combatState = creature.CombatState;

            if (deathWishPower == null || combatState == null || combatState.CurrentSide != CombatSide.Player || creature.Player == null)
            {
                return;
            }

            var randomAttack = CardFactory.GetDistinctForCombat(creature.Player, from c in creature.Player.Character.CardPool.GetUnlockedCards(creature.Player.UnlockState, creature.Player.RunState.CardMultiplayerConstraint)
                                                                                 where c.Type == CardType.Attack
                                                                                 select c, 1, creature.Player.RunState.Rng.CombatCardGeneration).FirstOrDefault();

            if (randomAttack == null)
            {
                return;
            }

            deathWishPower.Flash();
            GarbageSpaghettiCodeKurwaJebanaSzmataPierdolonaKurwaBezmozgiJebaneToPisaly(randomAttack.EnergyCost, 0);
            CardPileCmd.AddGeneratedCardToCombat(randomAttack, PileType.Hand, creature.Player);
        }

        private static void GarbageSpaghettiCodeKurwaJebanaSzmataPierdolonaKurwaBezmozgiJebaneToPisaly(CardEnergyCost cardEnergyCost, int cost, bool reduceOnly = false)
        {
            if (cost != 0 || cardEnergyCost.Canonical >= 0)
            {
                cardEnergyCost._localModifiers.Add(new LocalCostModifier(cost, LocalCostType.Absolute, LocalCostModifierExpiration.WhenPlayed, reduceOnly));
            }
        }
    }
}