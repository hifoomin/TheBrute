#region

using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using TheBrute.Character;

#endregion

namespace TheBrute.Cards
{
    [HarmonyPatch(typeof(TrashHeap), nameof(TrashHeap.Cards), MethodType.Getter)]
    internal class TrashHeapCardsPatch
    {
        private static CardModel[]? trashHeapCards;

        [HarmonyPostfix]
        private static void AddCustomCards(ref CardModel[] __result)
        {
            if (!Config.EnableTrashHeapAdditions)
            {
                return;
            }

            trashHeapCards ??=
            [
                .. ModelDb.CardPool<TheBruteCardPool>().AllCards.Where(card => card.Rarity == CardRarity.Event)
            ];
            if (trashHeapCards.Length == 0)
            {
                return;
            }

            __result =
            [
                .. __result,
                .. trashHeapCards
            ];
        }
    }

    [HarmonyPatch(typeof(TrashHeap), nameof(TrashHeap.Relics), MethodType.Getter)]
    internal class TrashHeapRelicsPatch
    {
        private static RelicModel[]? trashHeapRelics;

        [HarmonyPostfix]
        private static void AddCustomRelics(ref RelicModel[] __result)
        {
            if (!Config.EnableTrashHeapAdditions)
            {
                return;
            }

            trashHeapRelics ??=
            [
                .. ModelDb.RelicPool<TheBruteRelicPool>().AllRelics.Where(relic => relic.Rarity == RelicRarity.Event && relic.DynamicVars.ContainsKey("KurwaTrashHeap"))
            ];
            if (trashHeapRelics.Length == 0)
            {
                return;
            }

            __result =
            [
                .. __result,
                .. trashHeapRelics
            ];
        }
    }
}