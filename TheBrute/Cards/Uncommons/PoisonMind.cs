#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class PoisonMind : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public PoisonMind() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(6),
            new PowerVar<StrengthPower>(3m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<StrengthPower>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -DynamicVars.Strength.BaseValue, Owner.Creature, this);

                await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);

                VfxCmd.PlayOnCreature(cardPlay.Target, "vfx/vfx_slime_impact");
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}