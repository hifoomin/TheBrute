using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Powers;

namespace TheBrute.Cards.Multiplayer
{
    internal class Fervor : TheBruteCard
    {
        public Fervor() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(13m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new CardsVar(2),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);

            var alivePlayers = from c in CombatState.GetTeammatesOf(Owner.Creature)
                               where c != null && c.IsAlive && c.IsPlayer
                               select c;
            foreach (Creature player in alivePlayers)
            {
                UpgradeCards(PileType.Draw, player.Player);
                UpgradeCards(PileType.Discard, player.Player);
            }
        }

        private void UpgradeCards(PileType pileType, Player player)
        {
            var upgradeableCards = pileType.GetPile(player).Cards
                      .Where(card => card.IsUpgradable && card != this)
                      .ToList();

            var upgradeCount = Math.Min(DynamicVars.Cards.BaseValue, upgradeableCards.Count);

            for (int i = 0; i < upgradeCount; i++)
            {
                var randomUpgradeableCard = Owner.RunState.Rng.CombatCardSelection.NextItem(upgradeableCards);

                if (randomUpgradeableCard == null)
                {
                    continue;
                }

                CardCmd.Upgrade(randomUpgradeableCard, MegaCrit.Sts2.Core.Nodes.CommonUi.CardPreviewStyle.MessyLayout);
                upgradeableCards.Remove(randomUpgradeableCard);
                CardCmd.Preview(randomUpgradeableCard, 1.5f);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(2);
        }
    }
}