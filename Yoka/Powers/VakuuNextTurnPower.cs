using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoka.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class VakuuNextTurnPower : YokaPower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Single;

        private int gainedOnTurnNumber = 0;

        public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            gainedOnTurnNumber = applier.CombatState.RoundNumber;
            return Task.CompletedTask;
        }

        public override async Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner.Player)
            {
                return;
            }

            ICombatState combatState = player.Creature.CombatState;
            if (combatState.RoundNumber <= gainedOnTurnNumber)
            {
                return;
            }

            bool hasPlayedMaxCards = false;

            using (CardSelectCmd.PushSelector(new VakuuCardSelector()))
            {
                int cardsPlayed;
                for (cardsPlayed = 0; cardsPlayed < 13; cardsPlayed++)
                {
                    if (CombatManager.Instance.IsOverOrEnding)
                    {
                        break;
                    }

                    if (CombatManager.Instance.IsPlayerReadyToEndTurn(player))
                    {
                        break;
                    }

                    var handPile = PileType.Hand.GetPile(player);
                    var randomCard = handPile.Cards.FirstOrDefault((CardModel c) => c.CanPlay());
                    if (randomCard == null)
                    {
                        break;
                    }

                    var randomTarget = GetRandomTarget(randomCard, combatState, player);
                    await randomCard.SpendResources();
                    await CardCmd.AutoPlay(choiceContext, randomCard, randomTarget, AutoPlayType.Default, skipXCapture: true);
                }

                hasPlayedMaxCards = cardsPlayed >= 13;

                if (cardsPlayed == 0)
                {
                    return;
                }
            }
            var localizationString = (hasPlayedMaxCards ? new LocString("relics", "WHISPERING_EARRING.warning") : new LocString("relics", "WHISPERING_EARRING.approval"));
            TalkCmd.Play(localizationString, player.Creature, VfxColor.Purple);
            await PowerCmd.Remove(this);
        }

        private Creature? GetRandomTarget(CardModel card, ICombatState combatState, Player player)
        {
            Rng combatTargets = player.RunState.Rng.CombatTargets;
            return card.TargetType switch
            {
                TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
                TargetType.AnyAlly => combatTargets.NextItem(combatState.Allies.Where((Creature entry) => entry != null && entry.IsAlive && entry.IsPlayer && entry != player.Creature)),
                TargetType.AnyPlayer => player.Creature,
                _ => null,
            };
        }
    }
}