using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;
using TheBrute.Cards.Rares;
using TheBrute.Powers;

namespace TheBrute.Powers
{
    internal class BitterEmbracePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        // old: comment the 3 things below
        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(0),
        ];

        public void SetGeneratedCardsAmount(decimal amount)
        {
            AssertMutable();
            DynamicVars.Cards.BaseValue = amount;
        }

        public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
        {
            if (player != Owner.Player || Amount <= 0)
            {
                return;
            }

            var eligibleCards = ModelDb.Character<Character.TheBrute>().CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint)
                                .Where(card => AutoTag.goldRelatedCards.Contains(card.Id))
                                .Where(card =>
                                card.Rarity != CardRarity.Basic &&
                                card.Rarity != CardRarity.Ancient &&
                                card.Rarity != CardRarity.Status &&
                                card.Rarity != CardRarity.Token &&
                                card.Rarity != CardRarity.Curse &&
                                card != ModelDb.Card<BitterEmbrace>())
            .ToList();

            if (eligibleCards.Count > 0)
            {
                CardModel[] array = new CardModel[DynamicVars.Cards.IntValue];
                Rng combatCardGeneration = Owner.Player.RunState.Rng.CombatCardGeneration;
                for (int i = 0; i < DynamicVars.Cards.IntValue; i++) // old: set this to amount
                {
                    array[i] = CardFactory.GetDistinctForCombat(player, eligibleCards, 1, combatCardGeneration).First();
                }

                Flash();

                await CardPileCmd.AddGeneratedCardsToCombat(array, PileType.Hand, Owner.Player);
            }

            Amount--;
            // old, power ver: comment this

            // "THEBRUTE-BITTER_EMBRACE.description": "At the start of your turn, add {BitterEmbracePower:cond:>1?{BitterEmbracePower:diff()} random [gold]Gold[/gold] related {BitterEmbracePower:plural:card|cards}|a random [gold]Gold[/gold] related card} into your [gold]Hand[/gold].",

            // rip trout population
        }
    }
}