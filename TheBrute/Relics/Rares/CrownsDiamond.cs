#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using TheBrute.Cards;
using TheBrute.Character;

#endregion

namespace TheBrute.Relics.Rares
{
    internal class CrownsDiamond : TheBruteRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(2)
        ];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState!.TurnNumber <= 1)
            {
                IReadOnlyList<CardModel> eligibleCards =
                [
                    .. from card in ModelDb.CardPool<TheBruteCardPool>().GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                       where AutoTag.goldRelatedCards.Contains(card.Id)
                       select card
                ];

                if (eligibleCards.Count > 0)
                {
                    // big hat flashes twice, I wonder if it's a bug?
                    Flash();
                    var cards = CardFactory.GetDistinctForCombat(Owner, eligibleCards, DynamicVars.Cards.IntValue, Owner.RunState.Rng.CombatCardGeneration).ToList();
                    Flash();
                    await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner);
                }
            }
        }
    }
}