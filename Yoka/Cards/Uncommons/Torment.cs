using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Torment : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Torment() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override bool ShouldGlowGoldInternal => CombatManager.Instance.History.Entries
                .OfType<DamageReceivedEntry>()
                .Any(entry =>
                    entry.Actor.Player == Owner &&
                    entry.Dealer != null && entry.Dealer.IsEnemy &&
                    entry.RoundNumber == Owner.Creature.CombatState.RoundNumber - 1 &&
                    entry.Result.UnblockedDamage > 0
                );

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(11m, ValueProp.Move),
            new PowerVar<VulnerablePower>(2m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);

            if (ShouldGlowGoldInternal)
            {
                await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}