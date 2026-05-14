using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Uncommons
{
    internal class Poise : YokaCard
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
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            List<CardModel> cardsIn =
            [..
                (from c in PileType.Draw.GetPile(Owner).Cards
                orderby c.Rarity, c.Id
                select c)
            ];

            CardModel cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1))).FirstOrDefault();
            if (cardModel != null)
            {
                await CardCmd.Exhaust(choiceContext, cardModel);
            }

            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PlatingPower"].UpgradeValueBy(1m);
        }
    }
}