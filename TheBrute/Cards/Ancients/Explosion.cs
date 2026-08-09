#region

using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Ancients
{
    internal class Explosion : TheBruteCard, ITomeCard
    {
        public Explosion() : base(2, CardType.Power, CardRarity.Ancient, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new("ExtraAsPercent", 9m),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(0.09m),
            new CalculatedDamageVar(ValueProp.Unpowered).WithMultiplier((card, _) =>
            {
                return card.Owner.Creature.MaxHp;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<ExplosionPower>(choiceContext, Owner.Creature, DynamicVars.ExtraDamage.BaseValue * 100m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.ExtraDamage.UpgradeValueBy(0.04m);
            DynamicVars["ExtraAsPercent"].UpgradeValueBy(4m);
        }
    }
}