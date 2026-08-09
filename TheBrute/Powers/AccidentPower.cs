#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Powers
{
    internal class AccidentPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        /*
        public override async Task AfterGoldGained(Player player)
        {
            if (player != Owner.Player)
            {
                return;
            }

            var hittableEnemies = CombatManager.Instance._state?.HittableEnemies;
            if (hittableEnemies != null && hittableEnemies.Count > 0)
            {
                var randomEnemy = player.RunState.Rng.CombatTargets.NextItem(hittableEnemies);
                if (randomEnemy != null)
                {
                    Flash();
                    await CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), randomEnemy, Amount, ValueProp.Unpowered, null, null);
                }
            }
        }
        */

        // DONT USE THIS SHIT THE GAME DOESNT CALL IT OF COURSE HAHAFDSFHTATSHTA
    }

    [HarmonyPatch(typeof(PlayerCmd), "GainGold")]
    internal class AccidentPowerGainGoldPatch
    {
        private static void Postfix(Task __result, Player player)
        {
            _ = PostfixAsync(player);
        }

        private static async Task PostfixAsync(Player player)
        {
            var accidentPower = player.Creature.GetPower<AccidentPower>();
            var hittableEnemies = CombatManager.Instance._turnState?.State.HittableEnemies;
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

    [HarmonyPatch(typeof(PlayerCmd), "LoseGold")]
    internal class AccidentPowerLoseGoldPatch
    {
        private static void Postfix(Task __result, Player player)
        {
            _ = PostfixAsync(player);
        }

        private static async Task PostfixAsync(Player player)
        {
            var accidentPower = player.Creature.GetPower<AccidentPower>();
            var hittableEnemies = CombatManager.Instance._turnState?.State.HittableEnemies;
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