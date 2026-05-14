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
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Engorge : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Engorge() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
           CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(12),
            new MaxHpVar(4m),
            new DamageVar(25m, ValueProp.Move)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Fatal)];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");
            bool shouldTriggerFatal = cardPlay.Target.Powers.All((PowerModel p) => p.ShouldOwnerDeathTriggerFatal());
            AttackCommand attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bite", null, "blunt_attack.mp3")
                .Execute(choiceContext);
            if (shouldTriggerFatal && attackCommand.Results.SelectMany((List<DamageResult> r) => r).Any((DamageResult r) => r.WasTargetKilled))
            {
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.IntValue);
            }
            else
            {
                await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(10m);
        }
    }
}