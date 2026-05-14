using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Crash : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Crash() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.RandomEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m),
            new DamageVar(27m, ValueProp.Move)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).TargetingRandomOpponents(CombatState)
                .WithHitFx(null /*"vfx/vfx_attack_slash"*/)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(13m);
        }
    }
}