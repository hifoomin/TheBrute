#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

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
            Main.Audio.PlaySfx("impulse.ogg");

            await Cmd.Wait(0.8f);

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

            var attacksInDiscardPile = PileType.Discard.GetPile(Owner).Cards.Where(c => c.Type == CardType.Attack && !c.Keywords.Contains(CardKeyword.Unplayable)).ToList().StableShuffle(Owner.RunState.Rng.Shuffle);

            foreach (var card in attacksInDiscardPile)
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