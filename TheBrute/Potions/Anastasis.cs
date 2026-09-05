#region

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using TheBrute.Cards;
using TheBrute.Character;

#endregion

namespace TheBrute.Potions
{
    internal class Anastasis : TheBrutePotion
    {
        public override PotionRarity Rarity => PotionRarity.Rare;

        public override PotionUsage Usage => PotionUsage.CombatOnly;

        public override TargetType TargetType => TargetType.AnyPlayer;

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1)
        ];

        public override IEnumerable<IHoverTip> ExtraHoverTips { get; }

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            AssertValidForTargetedPotion(target);
            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("a9ffff"));

            await GenerateRandomCard(target, AutoTag.thornsRelatedCards);
            await GenerateRandomCard(target, AutoTag.goldRelatedCards);
            await GenerateRandomCard(target, AutoTag.maxHpRelatedCards);
        }

        private async Task GenerateRandomCard(Creature? target, HashSet<ModelId> cardHashSet)
        {
            var player = target?.Player;
            if (player == null)
            {
                return;
            }

            var eligibleCards = ModelDb.CardPool<TheBruteCardPool>().GetUnlockedCards(player.UnlockState, player.RunState.CardMultiplayerConstraint).Where(card => cardHashSet.Contains(card.Id)).Where(card => card.Rarity != CardRarity.Status && card.Rarity != CardRarity.Token && card.Rarity != CardRarity.Curse).ToList();

            // get distinct for combat -> filter for combat already checks whether it can be
            // generated in combat and that it ain't basic or ancient or event

            if (eligibleCards.Count > 0)
            {
                var combatCardGenerationRng = player.RunState.Rng.CombatCardGeneration;
                var randomCard = CardFactory.GetDistinctForCombat(player, eligibleCards, DynamicVars.Cards.IntValue, combatCardGenerationRng).First();
                randomCard.SetToFreeThisTurn();

                await CardPileCmd.AddGeneratedCardToCombat(randomCard, PileType.Hand, player);
            }
        }
    }
}