using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using Yoka.Powers;

namespace Yoka.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Degrade : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Degrade() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(5m),
            new ExtraDamageVar(2m),
            new CalculatedDamageVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            {
                return CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) =>
                       e.HappenedThisTurn(card.CombatState) &&
                       e.CardPlay.Card.Type == CardType.Attack &&
                       e.CardPlay.Card.Owner == card.Owner);
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(cardPlay.Target)).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
        }
    }
}