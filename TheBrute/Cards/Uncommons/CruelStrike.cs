#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class CruelStrike : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public CruelStrike() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override HashSet<CardTag> CanonicalTags => new([CardTag.Strike]);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(17m, ValueProp.Move),
            new PowerVar<VulnerablePower>(2m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<VulnerablePower>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_scratch")
                .Execute(choiceContext);

            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars.Vulnerable.UpgradeValueBy(1m);
        }
    }
}