#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Cryblood : TheBruteCard
    {
        public Cryblood() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        public override bool GainsBlock => false;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Block)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new("ExtraAsPercent", 4m),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(0.04m),
            new CalculatedBlockVar(ValueProp.Unpowered).WithMultiplier((card, _) =>
            {
                return card.Owner.Creature.MaxHp;
            })
        ];

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationExtra.UpgradeValueBy(0.02m);
            DynamicVars["ExtraAsPercent"].UpgradeValueBy(2m);
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CrybloodPower>(choiceContext, Owner.Creature, DynamicVars.CalculationExtra.BaseValue * 100m, Owner.Creature, this);
        }
    }
}