#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Malice : TheBruteCard
    {
        private decimal _extraDamageFromPlays;

        public Malice() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(18m, ValueProp.Move),
            new("Decrease", 2m)
        ];

        private decimal ExtraDamageFromPlays
        {
            get => _extraDamageFromPlays;
            set
            {
                AssertMutable();
                _extraDamageFromPlays = value;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/hellraiser_attack_vfx")
                .Execute(choiceContext);

            DynamicVars.Damage.BaseValue -= DynamicVars["Decrease"].BaseValue;
            ExtraDamageFromPlays -= DynamicVars["Decrease"].BaseValue;
        }

        protected override void AfterDowngraded()
        {
            base.AfterDowngraded();
            DynamicVars.Damage.BaseValue -= ExtraDamageFromPlays;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }

        protected override CardLocation GetResultLocationForCardPlay()
        {
            var resultLocationForCardPlay = base.GetResultLocationForCardPlay();
            if (resultLocationForCardPlay.pileType == PileType.Discard)
            {
                resultLocationForCardPlay.pileType = PileType.Draw;
                resultLocationForCardPlay.position = CardPilePosition.Top;
            }
            return resultLocationForCardPlay;
        }
    }
}