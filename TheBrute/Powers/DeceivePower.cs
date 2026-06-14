using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Powers
{
    internal class DeceivePower : TheBrutePower, IHasSecondAmount
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private class Data
        {
            public int skillsPlayedThisTurn;
        }

        protected override object InitInternalData()
        {
            return new Data();
        }

        public override Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            GetInternalData<Data>().skillsPlayedThisTurn = CombatManager.Instance.History.CardPlaysStarted.Count((CardPlayStartedEntry e) => e.CardPlay.Card.Type == CardType.Skill && e.CardPlay.Card.Owner.Creature == Owner && e.HappenedThisTurn(CombatState));
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
                for (int i = 0; i < Amount; i++)
                {
                    CardModel card = cardPlay.Card.CreateClone();
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

        public string GetSecondAmount()
        {
            var normalized = Math.Max(0, 3 - GetInternalData<Data>().skillsPlayedThisTurn);
            return $"{normalized}";
        }
    }
}