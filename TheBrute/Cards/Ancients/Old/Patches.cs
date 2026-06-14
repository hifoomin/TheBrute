using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Cards.Ancients;
using TheBrute.Cards.Starters;
using TheBrute.Relics.Ancients;
using TheBrute.Relics.Starters;
using TheBrute.Relics.Uncommons;

namespace TheBrute.Cards.Ancients
{
    internal class Patches
    {
    }

    /*
    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Relics.ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
    public class ArchaicToothPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Dictionary<ModelId, CardModel> __result)
        {
            __result[ModelDb.Card<KarmaStrike>().Id] = ModelDb.Card<Totality>();
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.Relics.TouchOfOrobas), "RefinementUpgrades", MethodType.Getter)]
    public class TouchOfOrobasPatch
    {
        [HarmonyPostfix]
        public static void Postfix(ref Dictionary<ModelId, RelicModel> __result)
        {
            __result[ModelDb.Relic<Toxemia>().Id] = ModelDb.Relic<Symbiosis>();
        }
    }
    */
}