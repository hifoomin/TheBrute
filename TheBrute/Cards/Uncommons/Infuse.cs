#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Infuse : TheBruteCard
    {
        public Infuse() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override bool ShouldGlowRedInternal => PileType.Draw.GetPile(Owner).Cards.Where(card => card.IsUpgradable && card != this).ToList().Count <= 0;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(1m),
            new CardsVar(5)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            var upgradeableCards = PileType.Draw.GetPile(Owner).Cards.Where(card => card.IsUpgradable && card != this).ToList();

            var upgradeCount = Math.Min(DynamicVars.Cards.IntValue, upgradeableCards.Count);

            for (var i = 0; i < upgradeCount; i++)
            {
                var randomUpgradeableCard = Owner.RunState.Rng.CombatCardSelection.NextItem(upgradeableCards);

                if (randomUpgradeableCard == null)
                {
                    continue;
                }

                CardCmd.Upgrade(randomUpgradeableCard, CardPreviewStyle.MessyLayout);
                upgradeableCards.Remove(randomUpgradeableCard);
                CardCmd.Preview(randomUpgradeableCard, 2.2f);
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}