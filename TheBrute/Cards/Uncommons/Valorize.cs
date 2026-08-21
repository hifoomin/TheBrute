#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Valorize : TheBruteCard
    {
        public Valorize() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(5),
            new PowerVar<StrengthPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await PlayerCmd.LoseGold(DynamicVars.Gold.BaseValue, Owner);

                await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}