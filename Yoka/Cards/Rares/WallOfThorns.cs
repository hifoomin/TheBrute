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

namespace Yoka.Cards.Rares
{
    internal class WallOfThorns : YokaCard
    {
        public WallOfThorns() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(30m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new PowerVar<ThornsPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].IntValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(10m);
            DynamicVars["ThornsPower"].UpgradeValueBy(1m);
        }
    }
}