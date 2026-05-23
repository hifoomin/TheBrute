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

namespace Yoka.Cards.Commons
{
    internal class Extend : YokaCard
    {
        public Extend() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<TemporaryThornsPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<TemporaryThornsPower>(2m),
            new PowerVar<ExtendPower>(2m)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            Yoka.Cards.Tags.thornsRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            (await PowerCmd.Apply<ExtendPower>(choiceContext, Owner.Creature, DynamicVars["ExtendPower"].BaseValue, Owner.Creature, this))?.SetTemporaryThornsAmount(DynamicVars["TemporaryThornsPower"].BaseValue);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["TemporaryThornsPower"].UpgradeValueBy(1m);
        }
    }
}