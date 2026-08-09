#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Poise : TheBruteCard
    {
        public Poise() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PlatingPower>(), HoverTipFactory.Static(StaticHoverTip.Block), HoverTipFactory.FromKeyword(CardKeyword.Exhaust)];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<PlatingPower>(6m),
            new CardsVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].IntValue, Owner.Creature, this);

            List<CardModel> cardsIn =
            [
                ..
                from c in PileType.Draw.GetPile(Owner).Cards
                orderby c.Rarity, c.Id
                select c
            ];

            var cards = await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, Owner, new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue));
            foreach (var card in cards)
            {
                await CardCmd.Exhaust(choiceContext, card);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PlatingPower"].UpgradeValueBy(2m);
        }
    }
}