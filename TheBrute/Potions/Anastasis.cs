using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;
using TheBrute.Potions;
using TheBrute.Powers;

namespace TheBrute.Potions
{
    internal class Anastasis : TheBrutePotion
    {
        public override PotionRarity Rarity => PotionRarity.Rare;

        public override PotionUsage Usage => PotionUsage.CombatOnly;

        public override TargetType TargetType => TargetType.Self;

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            PotionModel.AssertValidForTargetedPotion(target);
            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("a9ffff"));

            await GenerateRandomCard(target, Tags.thornsRelated);
            await GenerateRandomCard(target, Tags.goldRelated);
            await GenerateRandomCard(target, Tags.maxHpRelated);
        }

        private async Task GenerateRandomCard(Creature? target, CardTag cardTag)
        {
            var player = target.Player;
            if (player == null)
            {
                return;
            }

            var eligibleCards = player.Character.CardPool.GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where(delegate (CardModel card)
            {
                var cardRarity = card.Rarity;

                var hasTag = card.Tags.Contains(cardTag);

                var isAcceptableRarity = cardRarity != CardRarity.Basic || cardRarity != CardRarity.Ancient || cardRarity != CardRarity.Status || cardRarity != CardRarity.Token || cardRarity != CardRarity.Curse;

                var isEligible = hasTag && isAcceptableRarity;

                return isEligible;
            }).ToList();

            if (eligibleCards.Count > 0)
            {
                var combatCardGenerationRng = player.RunState.Rng.CombatCardGeneration;
                var randomCard = CardFactory.GetDistinctForCombat(player, eligibleCards, 1, combatCardGenerationRng).First();
                randomCard.SetToFreeThisTurn();

                await CardPileCmd.AddGeneratedCardToCombat(randomCard, PileType.Hand, player);
            }
        }
    }
}