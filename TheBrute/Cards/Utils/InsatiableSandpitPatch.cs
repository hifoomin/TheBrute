using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards
{
    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Powers.SandpitPower), "AfterRemoved")]
    internal class InsatiableSandpitPatch
    {
        private static void Postfix(MegaCrit.Sts2.Core.Models.Powers.SandpitPower __instance, Task __result, Creature oldOwner)
        {
            foreach (Creature zjedzony in __instance.AllAffectedCreatures)
            {
                if (zjedzony.IsPlayer || zjedzony.Monster is Osty)
                {
                    var thornsAmount = zjedzony.GetPowerAmount<ThornsPower>();
                    CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), __instance.Owner, thornsAmount, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move | MegaCrit.Sts2.Core.ValueProps.ValueProp.SkipHurtAnim, zjedzony);
                }
            }
        }
    }
}