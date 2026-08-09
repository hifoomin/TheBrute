#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class RazorFormPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
    internal class RazorFormPowerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, PlayerChoiceContext choiceContext, Creature creature)
        {
            var razorFormPower = creature.GetPower<RazorFormPower>();
            var combatState = creature.CombatState;

            if (razorFormPower != null && combatState != null)
            {
                razorFormPower.Flash();
                PowerCmd.Apply<ThornsPower>(choiceContext, creature, razorFormPower.Amount, creature, null);
            }
        }
    }
}