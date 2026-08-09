#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Unravel : TheBruteCard
    {
        public Unravel() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<UnravelPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await PowerCmd.Apply<UnravelPower>(choiceContext, Owner.Creature, DynamicVars["UnravelPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["UnravelPower"].UpgradeValueBy(1m);
        }
    }
}