#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using TheBrute.Cards;

#endregion

namespace TheBrute.Powers
{
    internal class SpineCharmPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.ForEnergy(this)
        ];

        protected override object InitInternalData()
        {
            return new Data();
        }

        /*
        public override Task BeforePowerAmountChanged(PowerModel power, decimal amount, Creature target, Creature? applier, CardModel? cardSource)
        {
            if (power != this)
            {
                return Task.CompletedTask;
            }
            HideTemporaryZeroCostVisual();
            return Task.CompletedTask;
        }

        public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            HideTemporaryZeroCostVisual();
            return Task.CompletedTask;
        }
        */

        public override bool TryModifyEnergyCostInCombatLate(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            modifiedCost = originalCost;

            if (ShouldSkip(card))
            {
                return false;
            }

            modifiedCost = 0;
            return true;
        }

        public override bool TryModifyStarCost(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            modifiedCost = originalCost;

            if (ShouldSkip(card))
            {
                return false;
            }

            modifiedCost = 0;
            return true;
        }

        private bool ShouldSkip(CardModel card)
        {
            if (card.Owner.Creature != Owner)
            {
                return true;
            }

            if (card.Pile?.Type is not (PileType.Hand or PileType.Play))
            {
                return true;
            }

            if (!AutoTag.thornsRelatedCards.Contains(card.Id))
            {
                return true;
            }

            if (card.EnergyCost.CostsX || card.HasStarCostX)
            {
                return true;
            }

            return GetInternalData<Data>().thornsRelatedCardsPlayedThisCombat >= Amount;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay != null && cardPlay.Card.Owner.Creature == Owner && !cardPlay.IsAutoPlay && cardPlay.IsLastInSeries && AutoTag.thornsRelatedCards.Contains(cardPlay.Card.Id) && !(cardPlay.Card.EnergyCost.CostsX || cardPlay.Card.HasStarCostX))
            {
                GetInternalData<Data>().thornsRelatedCardsPlayedThisCombat++;
                await PowerCmd.Decrement(this);
            }
        }

        public override Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (!participants.Contains(Owner))
            {
                return Task.CompletedTask;
            }

            if (Owner.Player is { PlayerCombatState.TurnNumber: 1 })
            {
                GetInternalData<Data>().thornsRelatedCardsPlayedThisCombat = 0;
            }

            return Task.CompletedTask;
        }

        private void HideTemporaryZeroCostVisual()
        {
            GetInternalData<Data>().thornsRelatedCardsPlayedThisCombat = 999999999;
        }

        private class Data
        {
            public int thornsRelatedCardsPlayedThisCombat;
        }
    }
}