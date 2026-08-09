#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Brace : TheBruteCard
    {
        public Brace() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new BlockVar(13m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            var pile = PileType.Discard.GetPile(Owner);
            var cards = await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, Owner, new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue));
            foreach (var card in cards)
            {
                if (card.Pile == null)
                {
                    continue;
                }

                if (card.Pile.Type != PileType.Discard)
                {
                    continue;
                }

                await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
            }
            /*
            bool flag = cardModel != null;
            bool flag2 = flag;
            if (flag2)
            {
                bool flag3;
                switch (cardModel.Pile?.Type)
                {
                    case PileType.Draw:
                    case PileType.Discard:
                        flag3 = true;
                        break;

                    default:
                        flag3 = false;
                        break;
                }
                flag2 = flag3;
            }
            if (flag2)
            {
                await CardPileCmd.Add(cardModel, PileType.Draw, CardPilePosition.Top);
            }
            */
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(4m);
        }
    }
}