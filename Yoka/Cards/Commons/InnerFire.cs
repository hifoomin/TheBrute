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

namespace Yoka.Cards.Commons
{
    internal class InnerFire : YokaCard
    {
        public InnerFire() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(1m),
            new BlockVar(11m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5m);
        }
    }
}