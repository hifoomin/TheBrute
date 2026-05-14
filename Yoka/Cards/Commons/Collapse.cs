using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Yoka.Powers;

namespace Yoka.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Collapse : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Collapse() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new HpLossVar(2m),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(2m),
            new CalculatedDamageVar(MegaCrit.Sts2.Core.ValueProps.ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            {
                return card.Owner.Creature.GetPowerAmount<ThornsPower>() + card.Owner.Creature.GetPowerAmount<TemporaryThornsPower>();
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
            await DamageCmd.Attack(((CalculatedDamageVar)DynamicVars["CalculatedDamage"]).Calculate(Owner.Creature)).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(1m);
        }
    }
}