/*
using BaseLib.Abstracts;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards
{
    public class HolyFuckingShitSchizo() : CustomSingletonModel(true, false)
    {
        public static readonly SpireField<Creature, Dictionary<(int turn, string moveId), decimal>> cachedCreatureBaseDamageToTurnToMoveIdMap = new(_ => []);

        public static readonly SpireField<Creature, Dictionary<(int turn, string moveId), int>> cachedCreatureHitCountToTurnToMoveIdMap = new(_ => []);

        public static decimal GetInitialBaseDamage(Creature creature, int turn, string moveId)
        {
            if (creature.CombatState == null)
            {
                return 0;
            }

            var dict = cachedCreatureBaseDamageToTurnToMoveIdMap[creature];
            return dict.TryGetValue((turn, moveId), out var value) ? value : 0;
        }

        public static int GetInitialHitCount(Creature creature, int turn, string moveId)
        {
            if (creature.CombatState == null)
            {
                return 0;
            }

            var dict = cachedCreatureHitCountToTurnToMoveIdMap[creature];
            return dict.TryGetValue((turn, moveId), out var value) ? value : 0;
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side != CombatSide.Player)
            {
                return;
            }

            foreach (var enemy in combatState.Enemies)
            {
                var move = enemy.Monster?.NextMove;
                if (move == null)
                {
                    continue;
                }

                var key = (combatState.RoundNumber, move.StateId);

                var attackIntent = move.Intents.OfType<AttackIntent>().FirstOrDefault();
                if (attackIntent == null)
                {
                    cachedCreatureBaseDamageToTurnToMoveIdMap[enemy][key] = 0;
                    cachedCreatureHitCountToTurnToMoveIdMap[enemy][key] = 0;
                    continue;
                }

                if (cachedCreatureBaseDamageToTurnToMoveIdMap[enemy].ContainsKey(key))
                {
                    continue;
                }

                var baseDamage = attackIntent.DamageCalc?.Invoke() ?? 0m;

                var baseHitCount = attackIntent is MultiAttackIntent multi ? multi.Repeats : 1;

                cachedCreatureBaseDamageToTurnToMoveIdMap[enemy][key] = baseDamage;
                cachedCreatureHitCountToTurnToMoveIdMap[enemy][key] = baseHitCount;
            }
        }

        public override async Task AfterCombatEnd(CombatRoom room)
        {
            foreach (Creature creature in room.Enemies)
            {
                cachedCreatureBaseDamageToTurnToMoveIdMap[creature].Clear();
                cachedCreatureHitCountToTurnToMoveIdMap[creature].Clear();
            }
        }
    }
}
*/