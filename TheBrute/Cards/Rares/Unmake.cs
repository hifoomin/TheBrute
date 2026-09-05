#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Unmake : TheBruteCard
    {
        public Unmake() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(0m),
            new CalculationExtraVar(2m),
            new CalculatedVar("CalculatedThorns").WithMultiplier((card, _) =>
            {
                return PileType.Hand.GetPile(card.Owner).Cards.Count(c => c != card);
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            var thorns = ((CalculatedVar)DynamicVars["CalculatedThorns"]).Calculate(Owner.Creature);

            var cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
            foreach (var card in cardsInHand)
            {
                // burning sticks lol
                if (card == this)
                {
                    continue;
                }

                await CardCmd.Exhaust(choiceContext, card);
            }

            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, thorns, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationExtra.UpgradeValueBy(1m);
        }
    }
}