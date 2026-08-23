#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Collapse : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Collapse() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new("ExtraAsPercent", 150m),
            new MaxHpVar(1m),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1.5m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier((card, _) =>
            {
                return card.Owner.Creature.GetPowerAmount<ThornsPower>();
            })
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            Main.Audio.PlaySfx("collapse.ogg");

            await DamageCmd.Attack(((CalculatedDamageVar)DynamicVars["CalculatedDamage"]).Calculate(Owner.Creature)).FromCard(this, cardPlay).Targeting(cardPlay.Target).Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(0.5m);
            DynamicVars["ExtraAsPercent"].UpgradeValueBy(50m);
        }
    }
}