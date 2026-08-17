#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Gnaw : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        private decimal _extraEverythingFromGnawPlays;

        public Gnaw() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
        }

        private decimal ExtraEverythingFromGnawPlays
        {
            get => _extraEverythingFromGnawPlays;
            set
            {
                AssertMutable();
                _extraEverythingFromGnawPlays = value;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(4m, ValueProp.Move),
            new RepeatVar(2),
            new CardsVar(1),
            new("EverythingIncrease", 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target).WithHitCount(DynamicVars.Repeat.IntValue)
                .BeforeDamage(() =>
                {
                    AudioUtils.PlayBite();
                    return Task.CompletedTask;
                })
                .Execute(choiceContext);

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

            var increase = DynamicVars["EverythingIncrease"].BaseValue;
            BuffFromGnawPlay(increase);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Repeat.UpgradeValueBy(1);
        }

        protected override void AfterDowngraded()
        {
            base.AfterDowngraded();

            EnergyCost.AddThisCombat((int)ExtraEverythingFromGnawPlays);

            DynamicVars.Damage.BaseValue += ExtraEverythingFromGnawPlays;
            DynamicVars.Repeat.BaseValue += ExtraEverythingFromGnawPlays;

            DynamicVars.Cards.BaseValue += ExtraEverythingFromGnawPlays;
        }

        private void BuffFromGnawPlay(decimal extraDamage)
        {
            EnergyCost.AddThisCombat((int)extraDamage);

            DynamicVars.Damage.BaseValue += extraDamage;
            DynamicVars.Repeat.BaseValue += extraDamage;

            DynamicVars.Cards.BaseValue += extraDamage;

            ExtraEverythingFromGnawPlays += extraDamage;
        }
    }
}