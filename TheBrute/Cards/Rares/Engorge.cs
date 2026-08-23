#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Engorge : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Engorge() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(12),
            new MaxHpVar(3m),
            new DamageVar(18m, ValueProp.Move)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Fatal)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            var shouldTriggerFatal = cardPlay.Target.Powers.All(p => p.ShouldOwnerDeathTriggerFatal());

            AudioUtils.PlayBite();

            var attackCommand = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_bite", null, "blunt_attack.mp3")
                .Execute(choiceContext);

            if (shouldTriggerFatal && attackCommand.Results.SelectMany(r => r).Any(r => r.WasTargetKilled))
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
            DynamicVars.Gold.UpgradeValueBy(3m);
            DynamicVars.MaxHp.UpgradeValueBy(1m);
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}