#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class BitterEmbrace : TheBruteCard
    {
        /*
        public BitterEmbrace() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<BitterEmbracePower>(1m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await PowerCmd.Apply<BitterEmbracePower>(choiceContext, Owner.Creature, DynamicVars["BitterEmbracePower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
        */

        public BitterEmbrace() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<BitterEmbracePower>(4m),
            new CardsVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            (await PowerCmd.Apply<BitterEmbracePower>(choiceContext, Owner.Creature, DynamicVars["BitterEmbracePower"].IntValue, Owner.Creature, this))?.SetGeneratedCardsAmount(DynamicVars.Cards.BaseValue);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["BitterEmbracePower"].UpgradeValueBy(2m);
        }
    }
}