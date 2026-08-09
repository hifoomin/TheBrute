#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheBrute.Character;

#endregion

namespace TheBrute.Cards
{
    [HarmonyPatch(typeof(ColorfulPhilosophers), "CardPoolColorOrder", MethodType.Getter)]
    internal class ColorfulPhilosophersPatch
    {
        private static void Postfix(ref IEnumerable<CardPoolModel> __result)
        {
            if (Config.EnableColorfulPhilosophersAdditions)
            {
                __result = __result.Append(ModelDb.CardPool<TheBruteCardPool>());
            }
        }
    }
}