#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Commons
{
    internal class Scheme : TheBruteCard
    {
        public Scheme() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(9m, ValueProp.Move),
            new PowerVar<TemporaryThornsUpNextTurnPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            await PowerCmd.Apply<TemporaryThornsUpNextTurnPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsUpNextTurnPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1m);
            DynamicVars["TemporaryThornsUpNextTurnPower"].UpgradeValueBy(2m);
        }
    }
}