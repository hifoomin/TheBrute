using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;

namespace TheBrute.Cards.Uncommons
{
    internal class Amend : TheBruteCard
    {
        public Amend() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Transform)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2),
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.maxHpRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            var transformableCards = PileType.Hand.GetPile(Owner).Cards.Where((CardModel c) => c != null && c.IsTransformable && (c.Type == CardType.Status || c.Type == CardType.Curse || c.Type == CardType.Quest)).ToList();
            foreach (CardModel transformableStatusCard in transformableCards)
            {
                var randomThornsCard = CardFactory.GetDistinctForCombat(Owner,
                                      from card in Owner.Character.CardPool.GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                                      where card.Tags.Contains(TheBrute.Cards.Tags.thornsRelated) &&
                                      card.Id != ModelDb.Card<Amend>().Id
                                      select card, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();

                // var toTransform = CombatState.CreateCard(randomZeroCostCard, Owner);
                await CardCmd.Transform(transformableStatusCard, randomThornsCard);
            }
            // holy fuck thiis might affect the trout population
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(-1m);
        }
    }
}