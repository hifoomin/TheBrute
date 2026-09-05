#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Autothysis : TheBruteCard
    {
        public Autothysis() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override bool IsPlayable => MaxHpTracker.GetTotalMaxHpLostThisCombat(Owner.Creature) >= DynamicVars["MaxHpLostRequirement"].BaseValue;
        protected override bool ShouldGlowRedInternal => !IsPlayable;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new("MaxHpLostRequirement", 4m),
            new BlockVar(15m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // just in case idk lmfaoo this game badly coded
            if (!IsPlayable)
            {
                return;
            }

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5m);
            // DynamicVars["MaxHpLostRequirement"].UpgradeValueBy(-1m);
        }
    }
}