#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class UnravelPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class UnravelPowerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, Creature creature, decimal amount, bool isFromCard)
        {
            var combatState = creature.CombatState;
            var unravelPower = creature.GetPower<UnravelPower>();
            var normalizedAmount = Math.Abs(amount); // idfk it shouldnt be negative anyway lmao
            if (combatState != null && unravelPower != null && normalizedAmount > 0)
            {
                PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), creature, unravelPower.Amount, creature, null);
            }
        }
    }
}