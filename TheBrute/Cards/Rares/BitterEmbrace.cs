#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class BitterEmbrace : TheBruteCard
    {
        public BitterEmbrace() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            var eligibleCards = ModelDb.Character<Character.TheBrute>().CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(card => AutoTag.goldRelatedCards.Contains(card.Id))
                .Where(card =>
                           card.Rarity != CardRarity.Status &&
                           card.Rarity != CardRarity.Token &&
                           card.Rarity != CardRarity.Curse &&
                           card != ModelDb.Card<BitterEmbrace>())
                .ToList();

            // get distinct for combat -> filter for combat already checks whether it can be
            // generated in combat and that it ain't basic or ancient or event

            if (eligibleCards.Count > 0)
            {
                var array = new CardModel[DynamicVars.Cards.IntValue];
                var combatCardGeneration = Owner.RunState.Rng.CombatCardGeneration;
                for (var i = 0; i < DynamicVars.Cards.IntValue; i++)
                {
                    array[i] = CardFactory.GetDistinctForCombat(Owner, eligibleCards, 1, combatCardGeneration).First();
                    array[i].SetToFreeThisTurn();
                    if (IsUpgraded)
                    {
                        CardCmd.Upgrade(array[i]);
                    }
                }

                await CardPileCmd.AddGeneratedCardsToCombat(array, PileType.Hand, Owner);
            }
        }
    }
}