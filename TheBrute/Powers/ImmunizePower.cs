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
    internal class ImmunizePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }

    [HarmonyPatch(typeof(CreatureCmd), "LoseMaxHp")]
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

                foreach (var player in alivePlayersExcludingPowerOwner)
                {
                    CreatureCmd.GainMaxHp(player, immunizePower.Amount);
                }

                immunizePower.Flash();
                PowerCmd.Apply<ThornsPower>(choiceContext, creature, immunizePower.Amount, null, null);
            }
        }
    }
}