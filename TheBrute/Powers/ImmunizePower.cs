using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Powers
{
    internal class ImmunizePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.CreatureCmd), "LoseMaxHp")]
    internal class ImmunizePowerLoseMaxHpPatch
    {
        private static void Postfix(Task __result, PlayerChoiceContext choiceContext, Creature creature)
        {
            var immunizePower = creature.GetPower<ImmunizePower>();
            var combatState = creature.CombatState;

            if (immunizePower != null && combatState != null)
            {
                var alivePlayersExcludingPowerOwner = from c in combatState.GetTeammatesOf(creature)
                                                      where c != null && c.IsAlive && c.IsPlayer
                                                      && c != creature
                                                      select c;

                foreach (Creature player in alivePlayersExcludingPowerOwner)
                {
                    CreatureCmd.GainMaxHp(player, immunizePower.Amount);
                }

                immunizePower.Flash();
                PowerCmd.Apply<ThornsPower>(choiceContext, creature, immunizePower.Amount, null, null);
            }
        }
    }
}