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
    internal class Accumulate : YokaCard
    {
        public Accumulate() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<StrengthPower>(1m),
            new BlockVar(11m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}