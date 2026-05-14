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

namespace Yoka.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Beating : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Beating() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(2m, ValueProp.Move),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, Creature? _) =>
            {
                return CombatManager.Instance.History.Entries.OfType<DamageReceivedEntry>().Count((DamageReceivedEntry e) => e.Receiver == card.Owner.Creature && e.Result.UnblockedDamage > 0);
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount((int)((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(Owner.Creature)).FromCard(this)
            .Targeting(cardPlay.Target)
            .WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}