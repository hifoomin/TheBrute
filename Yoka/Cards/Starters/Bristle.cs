using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Starters
{
    internal class Bristle : YokaCard
    {
        public Bristle() : base(0, CardType.Skill, CardRarity.Basic, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TemporaryThornsPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<Powers.TemporaryThornsPower>(4m)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<Powers.TemporaryThornsPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["TemporaryThornsPower"].UpgradeValueBy(1m);
            AddKeyword(CardKeyword.Retain);
        }
    }
}