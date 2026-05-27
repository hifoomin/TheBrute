using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards.Uncommons
{
    internal class Infuse : TheBruteCard
    {
        public Infuse() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m),
            new CardsVar(5)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            TheBrute.Cards.Tags.maxHpRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            var upgradeableCards = PileType.Draw.GetPile(Owner).Cards
                                  .Where(card => card.IsUpgradable && card != this)
                                  .ToList();

            var upgradeCount = Math.Min(DynamicVars.Cards.IntValue, upgradeableCards.Count);

            for (int i = 0; i < upgradeCount; i++)
            {
                var randomUpgradeableCard = Owner.RunState.Rng.CombatCardSelection.NextItem(upgradeableCards);

                if (randomUpgradeableCard == null)
                {
                    continue;
                }

                CardCmd.Upgrade(randomUpgradeableCard, MegaCrit.Sts2.Core.Nodes.CommonUi.CardPreviewStyle.MessyLayout);
                upgradeableCards.Remove(randomUpgradeableCard);
                CardCmd.Preview(randomUpgradeableCard, 2.5f);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(-1m);
        }
    }
}