using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;
using TheBrute.Powers;

namespace TheBrute.Powers
{
    internal class BitterEmbracePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
        {
            if (player != Owner.Player)
            {
                return;
            }

            var eligibleCards = Owner.Player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where(delegate (CardModel card)
            {
                var cardRarity = card.Rarity;

                var isGoldRelated = card.Tags.Contains(Tags.goldRelated);

                var isAcceptableRarity = cardRarity != CardRarity.Basic || cardRarity != CardRarity.Ancient || cardRarity != CardRarity.Status || cardRarity != CardRarity.Token || cardRarity != CardRarity.Curse;

                var isEligible = isGoldRelated && isAcceptableRarity;

                return isEligible;
            }).ToList();

            if (eligibleCards.Count > 0)
            {
                CardModel[] array = new CardModel[Amount];
                Rng combatCardGeneration = Owner.Player.RunState.Rng.CombatCardGeneration;
                for (int i = 0; i < Amount; i++)
                {
                    array[i] = CardFactory.GetDistinctForCombat(player, eligibleCards, 1, combatCardGeneration).First();
                }

                Flash();

                await CardPileCmd.AddGeneratedCardsToCombat(array, PileType.Hand, Owner.Player);
            }

            // rip trout population
        }
    }
}