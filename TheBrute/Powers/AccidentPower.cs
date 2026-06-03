using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Relics;

namespace TheBrute.Powers
{
    internal class AccidentPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "GainGold")]
    internal class GainGoldPatch
    {
        private static void Postfix(Task __result, Player player)
        {
            _ = PostfixAsync(player);
        }

        private static async Task PostfixAsync(Player player)
        {
            var accidentPower = player.Creature.GetPower<AccidentPower>();
            var hittableEnemies = CombatManager.Instance._state?.HittableEnemies;
            if (accidentPower != null && hittableEnemies != null && hittableEnemies.Count > 0)
            {
                var randomEnemy = player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                if (randomEnemy != null)
                {
                    accidentPower.Flash();
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), randomEnemy, accidentPower.Amount, ValueProp.Unpowered, null, null);
                }
            }
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "LoseGold")]
    internal class LoseGoldPatch
    {
        private static void Postfix(Task __result, Player player)
        {
            _ = PostfixAsync(player);
        }

        private static async Task PostfixAsync(Player player)
        {
            var accidentPower = player.Creature.GetPower<AccidentPower>();
            var hittableEnemies = CombatManager.Instance._state?.HittableEnemies;
            if (accidentPower != null && hittableEnemies != null && hittableEnemies.Count > 0)
            {
                var randomEnemy = player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                if (randomEnemy != null)
                {
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), randomEnemy, accidentPower.Amount, ValueProp.Unpowered, null, null);
                }
            }
        }
    }
}