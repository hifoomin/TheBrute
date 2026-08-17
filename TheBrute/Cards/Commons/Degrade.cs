#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Degrade : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Degrade() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowGoldInternal => ThornsTracker.GetGainedThornsThisTurn(Owner.Creature);

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(5m, ValueProp.Move),
            new RepeatVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            var hitCount = ThornsTracker.GetGainedThornsThisTurn(Owner.Creature) ? DynamicVars.Repeat.IntValue : 1;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(hitCount).FromCard(this, cardPlay)
                .Targeting(cardPlay.Target)
                .WithHitFx()
                .BeforeDamage(() =>
                {
                    AudioUtils.PlayPunch();
                    return Task.CompletedTask;
                })
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}