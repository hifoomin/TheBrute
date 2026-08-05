using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.ValueProps;

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Impulse : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Impulse() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var cardsInHand = PileType.Hand.GetPile(Owner).Cards.ToList();
            foreach (CardModel card in cardsInHand)
            {
                // burning sticks lol
                if (card == this)
                {
                    continue;
                }

                await CardCmd.Exhaust(choiceContext, card);
            }

            var attacksInDiscardPile = PileType.Discard.GetPile(Owner).Cards.Where((CardModel c) => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle);

            foreach (CardModel card in attacksInDiscardPile)
            {
                if (!CombatManager.Instance.IsOverOrEnding)
                {
                    if (card.TargetType == TargetType.AnyEnemy)
                    {
                        var randomTarget = Owner.RunState.Rng.CombatTargets.NextItem(CombatState.HittableEnemies);
                        await CardCmd.AutoPlay(choiceContext, card, randomTarget);
                    }
                    else
                    {
                        await CardCmd.AutoPlay(choiceContext, card, null);
                    }
                    continue;
                }
                break;
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}