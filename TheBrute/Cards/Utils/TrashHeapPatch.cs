using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            trashHeapCards ??= [.. ModelDb.CardPool<Character.TheBruteCardPool>().AllCards.Where(card => card.Rarity == MegaCrit.Sts2.Core.Entities.Cards.CardRarity.Event)];
            if (trashHeapCards.Length == 0)
            {
                return;
            }

            __result = [.. __result, .. trashHeapCards];
        }
    }
}