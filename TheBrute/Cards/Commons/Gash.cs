#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Commons
{
    internal class Gash : TheBruteCard
    {
        public Gash() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowGoldInternal => MaxHpTracker.GetChangedMaxHpThisTurn(Owner.Creature);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(17m),
            new ExtraDamageVar(7m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, target) =>
            {
                return MaxHpTracker.GetChangedMaxHpThisTurn(card.Owner.Creature) ? 1m : 0m;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            var result = await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .Execute(choiceContext);

            AudioUtils.PlaySlash(result);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(4m);
        }
    }
}