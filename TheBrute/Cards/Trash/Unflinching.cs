#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Character;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Trash
{
    [Pool(typeof(EventCardPool))]
    internal class Unflinching : TheBruteCard
    {
        public Unflinching() : base(2, CardType.Power, CardRarity.Event, TargetType.Self)
        {
        }

        public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheBruteCardPool>();

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<PlatingPower>(7m),
            new PowerVar<ReducedMaximumHandSizePower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].BaseValue, Owner.Creature, this);

            await PowerCmd.Apply<ReducedMaximumHandSizePower>(choiceContext, Owner.Creature, DynamicVars["ReducedMaximumHandSizePower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PlatingPower"].UpgradeValueBy(2m);
        }
    }
}