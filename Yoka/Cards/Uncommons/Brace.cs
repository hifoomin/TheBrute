using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Cards.Uncommons
{
    internal class Brace : YokaCard
    {
        public Brace() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(14m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            CardSelectorPrefs prefs = new CardSelectorPrefs(base.SelectionScreenPrompt, 1);
            CardPile pile = PileType.Discard.GetPile(base.Owner);
            CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, pile.Cards, base.Owner, prefs)).FirstOrDefault();
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
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(4m);
        }
    }
}