using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class BindingChains : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public BindingChains() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, 15);

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(15),
            new PowerVar<StrengthPower>(2m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            if (Utils.HasGold(Owner, 15))
            {
                await PowerCmd.Apply<StrengthPower>(choiceContext, cardPlay.Target, -DynamicVars.Strength.BaseValue, Owner.Creature, this);
                await PlayerCmd.LoseGold(DynamicVars["Gold"].IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Gold.UpgradeValueBy(-5m);
        }
    }
}