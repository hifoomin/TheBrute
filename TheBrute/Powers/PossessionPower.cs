#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Nodes.Vfx;

#endregion

namespace TheBrute.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class PossessionPower : TheBrutePower
#pragma warning restore STS001 // Symbol missing localization
    {
        private int activatedOnTurnNumber;
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override Task BeforeApplied(Creature target, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (Amount <= 0 && applier.CombatState != null)
            {
                activatedOnTurnNumber = applier.CombatState.RoundNumber + 1;
            }

            return Task.CompletedTask;
        }

        public override async Task AfterAutoPrePlayPhaseEnteredLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner.Player)
            {
                return;
            }

            var combatState = player.Creature.CombatState;
            if (combatState.RoundNumber < activatedOnTurnNumber)
            {
                return;
            }

            var hasPlayedMaxCards = false;

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
                    var leftmostCard = handPile.Cards.FirstOrDefault(c => c.CanPlay());
                    if (leftmostCard == null)
                    {
                        break;
                    }

                    var randomTarget = GetRandomTarget(leftmostCard, combatState, player);
                    await leftmostCard.SpendResources();
                    await CardCmd.AutoPlay(choiceContext, leftmostCard, randomTarget, AutoPlayType.Default, true);
                }

                hasPlayedMaxCards = cardsPlayed >= 13;

                if (cardsPlayed == 0)
                {
                    return;
                }
            }
            var localizationString = hasPlayedMaxCards ? new LocString("relics", "WHISPERING_EARRING.warning") : new LocString("relics", "WHISPERING_EARRING.approval");
            TalkCmd.Play(localizationString, player.Creature, VfxColor.Purple);

            await PowerCmd.Decrement(this);
        }

        private Creature? GetRandomTarget(CardModel card, ICombatState combatState, Player player)
        {
            var combatTargets = player.RunState.Rng.CombatTargets;
            return card.TargetType switch
            {
                TargetType.AnyEnemy => combatState.HittableEnemies.FirstOrDefault(),
                TargetType.AnyAlly => combatTargets.NextItem(combatState.Allies.Where(c => c != null && c.IsAlive && c.IsPlayer && c != player.Creature)),
                TargetType.AnyPlayer => player.Creature,
                _ => null
            };
        }
    }
}