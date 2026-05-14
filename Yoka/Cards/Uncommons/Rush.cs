using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Cards.Uncommons
{
    internal class Rush : YokaCard
    {
        public Rush() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new PowerVar<VigorPower>(4m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            CardModel cardModel = (await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner)).FirstOrDefault();
            if (cardModel != null && cardModel.Type == CardType.Attack)
            {
                await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, DynamicVars["VigorPower"].BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["VigorPower"].UpgradeValueBy(2m);
        }
    }
}