using MegaCrit.Sts2.Core.Combat;
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

namespace Yoka.Powers
{
    internal class DeathWishPower : YokaPower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        private int lastMaxHP;

        public override async Task BeforeCardPlayed(CardPlay cardPlay)
        {
            lastMaxHP = Owner.MaxHp;
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner.Player && lastMaxHP > Owner.MaxHp)
            {
                CardModel cardModel = CardFactory.GetDistinctForCombat(Owner.Player,
                                      from c in Owner.Player.Character.CardPool.GetUnlockedCards(Owner.Player.UnlockState, Owner.Player.RunState.CardMultiplayerConstraint)
                                      where c.Type == CardType.Attack
                                      select c, 1, Owner.Player.RunState.Rng.CombatCardGeneration).FirstOrDefault();

                if (cardModel != null)
                {
                    cardModel.SetToFreeThisTurn();
                    await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner.Player);
                }
            }
        }
    }
}