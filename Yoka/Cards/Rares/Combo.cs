using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Combo : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Combo() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ComboPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await PowerCmd.Apply<ComboPower>(choiceContext, Owner.Creature, DynamicVars["ComboPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ComboPower"].UpgradeValueBy(1m);
        }
    }
}