#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Heave : TheBruteCard
    {
        public Heave() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromKeyword(CardKeyword.Exhaust)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new GoldVar(2)
        ];

        /*
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];
        */

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var exhaustRepeats = ResolveEnergyXValue();
            if (IsUpgraded)
            {
                exhaustRepeats++;
            }

            var goldLossRepeats = ResolveEnergyXValue();

            List<CardModel> cardsIn =
            [
                ..
                from c in PileType.Draw.GetPile(Owner).Cards
                orderby c.Rarity, c.Id
                select c
            ];

            if (cardsIn.Count > 0 && (goldLossRepeats > 0 || IsUpgraded))
            {
                foreach (var card in await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars.Cards.IntValue * exhaustRepeats)))
                {
                    if (card != null)
                    {
                        await CardCmd.Exhaust(choiceContext, card);
                    }
                }

                for (var i = 0; i < goldLossRepeats; i++)
                {
                    await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner);
                }
            }
        }
    }
}