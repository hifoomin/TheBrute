#region

using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Powers
{
    internal class DeceivePower : TheBrutePower, IHasSecondAmount
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public string GetSecondAmount()
        {
            var normalized = Math.Max(0, 3 - GetInternalData<Data>().skillsPlayedThisTurn);
            return $"{normalized}";
        }

        protected override object InitInternalData()
        {
            return new Data();
        }

        public override Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            GetInternalData<Data>().skillsPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count(e => e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
            this.InvokeSecondAmountChanged();
            return Task.CompletedTask;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner != Owner.Player || cardPlay.Card.Type != CardType.Skill)
            {
                return;
            }
            GetInternalData<Data>().skillsPlayedThisTurn++;
            this.InvokeSecondAmountChanged();
            if (GetInternalData<Data>().skillsPlayedThisTurn == 3)
            {
                Flash();
                for (var i = 0; i < Amount; i++)
                {
                    var card = cardPlay.Card.CreateClone();
                    await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
                }
            }
        }

        public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            if (participants.Contains(Owner))
            {
                GetInternalData<Data>().skillsPlayedThisTurn = 0;
                this.InvokeSecondAmountChanged();
            }
        }

        private class Data
        {
            public int skillsPlayedThisTurn;
        }
    }
}