#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class HighRollerPower : TheBrutePower
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

            var combatState = player.Creature.CombatState;
            if (combatState != null)
            {
                Flash();
                await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, Amount, player.Creature, null);
            }
        }
        */

        // DONT USE THIS SHIT THE GAME DOESNT CALL IT OF COURSE HAHAFDSFHTATSHTA
    }

    [HarmonyPatch(typeof(PlayerCmd), "GainGold")]
    internal class HighRollerPowerGainGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player)
        {
            var combatState = player.Creature.CombatState;
            var highRollerPower = player.Creature.GetPower<HighRollerPower>();
            if (combatState != null && highRollerPower != null)
            {
                highRollerPower.Flash();
                PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, highRollerPower.Amount, player.Creature, null);
            }
        }
    }

    [HarmonyPatch(typeof(PlayerCmd), "LoseGold")]
    internal class HighRollerPowerLoseGoldPatch
    {
        private static void Postfix(Task __result, decimal amount, Player player, GoldLossType goldLossType)
        {
            var combatState = player.Creature.CombatState;
            var highRollerPower = player.Creature.GetPower<HighRollerPower>();
            if (combatState != null && highRollerPower != null)
            {
                highRollerPower.Flash();
                PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), player.Creature, highRollerPower.Amount, player.Creature, null);
            }
        }
    }
}