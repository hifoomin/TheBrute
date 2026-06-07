using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Overkill : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Overkill() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(9m, ValueProp.Move),
            new RepeatVar(2),
            new PowerVar<WeakPower>(3m),
            new PowerVar<StrengthPower>(2m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Fatal),
            HoverTipFactory.FromPower<StrengthPower>(),
            HoverTipFactory.FromPower<WeakPower>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .WithHitFx("vfx/vfx_heavy_blunt", null, "heavy_attack.mp3")
                .Execute(choiceContext);

            if (attackCommand.Results.SelectMany((List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled) && CombatState != null)
            {
                for (int i = 0; i < CombatState.Enemies.Count; i++)
                {
                    var enemy = CombatState.Enemies[i];
                    await PowerCmd.Apply<StrengthPower>(choiceContext, enemy, -DynamicVars.Strength.BaseValue, Owner.Creature, this);
                    await PowerCmd.Apply<WeakPower>(choiceContext, enemy, DynamicVars.Weak.BaseValue, Owner.Creature, this);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
            DynamicVars.Weak.UpgradeValueBy(2m);
        }
    }
}