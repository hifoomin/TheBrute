#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Hysteria : TheBruteCard
    {
        public Hysteria() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new EnergyVar(1),
            new PowerVar<HysteriaPower>(1m),
            new MaxHpVar(1m),
            new GoldVar(5)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            EnergyHoverTip
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            (await PowerCmd.Apply<HysteriaPower>(choiceContext, Owner.Creature, DynamicVars["HysteriaPower"].BaseValue, Owner.Creature, this))?.SetLossAmounts(DynamicVars.MaxHp.BaseValue, DynamicVars.Gold.IntValue);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}