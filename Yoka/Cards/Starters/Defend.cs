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

namespace Yoka.Cards.Starters
{
    internal class Defend : YokaCard
    {
        public Defend() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override HashSet<CardTag> CanonicalTags => new([CardTag.Defend]);

        protected override IEnumerable<DynamicVar> CanonicalVars => [new BlockVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}