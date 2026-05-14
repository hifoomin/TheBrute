using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Victimize : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Victimize() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<VulnerablePower>(2m),
            new GoldVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
            await PlayerCmd.GainGold(DynamicVars["Gold"].IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Gold.UpgradeValueBy(1m);
        }
    }
}