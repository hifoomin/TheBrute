using BaseLib.Patches.Content;
using BaseLib.Patches.Features;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Godot.Performance;

namespace TheBrute.Cards
{
    public static class TargetTypes
    {
        [CustomEnum]
        public static TargetType MoreThanOneBaseDamageAttackIntent;
    }

    [HarmonyPatch(typeof(ModelDb), "Init")]
    internal static class ModelDbTargetTypeInitPatch
    {
        [HarmonyPostfix]
        private static void RegisterTargetTypes()
        {
            CustomTargetType.RegisterSingleTargetType(TargetTypes.MoreThanOneBaseDamageAttackIntent, HasMoreThanOneBaseDamageAttackIntent);
        }

        private static bool HasMoreThanOneBaseDamageAttackIntent(Creature target)
        {
            if (target is not { IsAlive: true, IsEnemy: true })
            {
                return false;
            }

            var monster = target.Monster;
            if (monster?.NextMove == null)
            {
                return false;
            }

            foreach (var intent in monster.NextMove.Intents)
            {
                if (intent is AttackIntent attack && attack.DamageCalc?.Invoke() > 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}