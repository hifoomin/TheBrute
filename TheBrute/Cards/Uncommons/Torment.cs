#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Torment : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Torment() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowGoldInternal => GoldTracker.GetChangedGoldThisTurn(Owner.Creature);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(11m, ValueProp.Move),
            new GoldVar(3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            AudioUtils.PlayPunch();

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .Execute(choiceContext);

            if (GoldTracker.GetChangedGoldThisTurn(Owner.Creature))
            {
                await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
            DynamicVars.Gold.UpgradeValueBy(1m);
        }
    }
}