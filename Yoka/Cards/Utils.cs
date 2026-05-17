using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards.Ancients;
using Yoka.Cards.Starters;
using Yoka.Relics.Uncommons;

namespace Yoka
{
    public static class Utils
    {
        public static bool TookUnblockedDamageLastTurn(Player owner)
        {
            return CombatManager.Instance.History.Entries
                .OfType<DamageReceivedEntry>()
                .Any(entry =>
                    entry.Actor.Player == owner &&
                    entry.Dealer != null && entry.Dealer.IsEnemy &&
                    entry.RoundNumber == owner.Creature.CombatState.RoundNumber - 1 &&
                    entry.Result.UnblockedDamage > 0);
        }

        public static int TookUnblockedDamageCount(Player owner)
        {
            int count = 0;
            foreach (CombatHistoryEntry combatHistoryEntry in CombatManager.Instance.History.Entries)
            {
                if (combatHistoryEntry is not DamageReceivedEntry damageReceivedEntry)
                {
                    continue;
                }

                if (damageReceivedEntry.Actor.Player == owner && damageReceivedEntry.Dealer != null && damageReceivedEntry.Result.UnblockedDamage > 0)
                {
                    count++;
                }
            }
            return count;
        }

        public static IEnumerable<CardModel> GetAllCardsExceptExhaustPile(Player owner)
        {
            return owner.PlayerCombatState._piles
                  .Where(p => p.Type != PileType.Exhaust)
                  .SelectMany(p => p.Cards);
        }

        public static bool HasGold(Player owner, int amount)
        {
            return owner.Gold >= amount;
        }
    }
}