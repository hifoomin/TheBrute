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
using TheBrute.Powers;

namespace TheBrute.Cards.Commons
{
    internal class Extend : TheBruteCard
    {
        public Extend() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(4m),
            new PowerVar<TemporaryThornsUpPower>(4m),
            new PowerVar<ExtendPower>(2m)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.thornsRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            (await PowerCmd.Apply<ExtendPower>(choiceContext, Owner.Creature, DynamicVars["ExtendPower"].BaseValue, Owner.Creature, this))?.SetTemporaryThornsUpAmount(DynamicVars["TemporaryThornsUpPower"].BaseValue);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ExtendPower"].UpgradeValueBy(1m);
        }
    }
}