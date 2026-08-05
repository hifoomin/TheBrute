/*
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Ancients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute
{
    internal class KurwaJebaneGownoPierdoloneKurwaJebaneKurwaChujstwoKurwa
    {
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Entities.Ancients.AncientDialogueSet), "GetAllDialogues")]
    public class KurwaJebanaUsunKurwaVanilleKurwaSzmatoKurwaJebanaKurwaPierdolonaKurwaPizdoKurwaJebanaKurwa
    {
        [HarmonyPrefix]
        public static bool Prefix(ref IEnumerable<AncientDialogue> __result, AncientDialogueSet __instance)
        {
            __result = KurwaJebanaJegoMac(__instance);
            return false;
        }

        private static IEnumerable<AncientDialogue> KurwaJebanaJegoMac(AncientDialogueSet __instance)
        {
            if (__instance.FirstVisitEverDialogue != null)
            {
                yield return __instance.FirstVisitEverDialogue;
            }
            else
            {
                Main.Logger.Warn("first visit ever dialogue is null");
            }

            foreach (IReadOnlyList<AncientDialogue> value in __instance.CharacterDialogues.Values)
            {
                foreach (AncientDialogue item in value)
                {
                    Main.Logger.Warn("added " + item.Lines);
                    yield return item;
                }
            }
        }
    }
}
*/