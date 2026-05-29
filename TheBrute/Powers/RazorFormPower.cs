using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards;
using TheBrute.Relics.Uncommons;

namespace TheBrute.Powers
{
    internal class RazorFormPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CreatureCmd), "LoseMaxHp")]
    internal class LoseMaxHpPatch
    {
        private static void Postfix(Task __result, PlayerChoiceContext choiceContext, Creature creature)
        {
            var razorFormPower = creature.GetPower<RazorFormPower>();
            var combatState = creature.CombatState;

            if (razorFormPower != null && combatState != null)
            {
                razorFormPower.Flash();
                PowerCmd.Apply<ThornsPower>(choiceContext, creature, razorFormPower.Amount, null, null);
            }
        }
    }
}