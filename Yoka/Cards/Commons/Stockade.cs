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

namespace Yoka.Cards.Commons
{
    internal class Stockade : YokaCard
    {
        public Stockade() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>(),
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(1m),
            new CardsVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}