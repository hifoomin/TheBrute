#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Monsters;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards
{
    [HarmonyPatch(typeof(SandpitPower), "AfterRemoved")]
    internal class InsatiableSandpitPatch
    {
        private static void Postfix(SandpitPower __instance, Task __result, Creature oldOwner)
        {
            foreach (var zjedzony in __instance.AllAffectedCreatures)
            {
                if (zjedzony.IsPlayer || zjedzony.Monster is Osty)
                {
                    var thornsAmount = zjedzony.GetPowerAmount<ThornsPower>();
                    CreatureCmd.Damage(new ThrowingPlayerChoiceContext(), __instance.Owner, thornsAmount, ValueProp.Move | ValueProp.SkipHurtAnim, zjedzony);
                }
            }
        }
    }
}