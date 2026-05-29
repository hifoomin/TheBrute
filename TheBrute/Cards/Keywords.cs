using BaseLib.Patches.Content;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Relics;

namespace TheBrute.Cards
{
    public static class Keywords
    {
        [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
        public static CardKeyword transmutedKeyword;

        public static bool hasTransmutedKeyword(this CardModel card)
        {
            return card.Keywords.Contains(transmutedKeyword);
        }

        [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
        public static CardKeyword goldLossKeyword;

        public static bool hasGoldLossKeyword(this CardModel card)
        {
            return card.Keywords.Contains(goldLossKeyword);
        }
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Models.CardModel), "OnPlayWrapper")]
    public class OnPlayWrapperPatch
    {
        private static async Task Postfix(Task __result, PlayerChoiceContext choiceContext, CardModel __instance)
        {
            await __result;

            // Main.Logger.Warn("onplaywrapperpatch ran");

            if (__instance.hasTransmutedKeyword())
            {
                // Main.Logger.Warn("CARD HAS TRANSMUTED KEYWORD, PLAYINGGGGG");
                // await CreatureCmd.LoseMaxHp(choiceContext, __instance.Owner.Creature, __instance.DynamicVars.MaxHp.BaseValue, true);
                CreatureCmd.LoseMaxHp(choiceContext, __instance.Owner.Creature, __instance.DynamicVars["Transmuted"].BaseValue, true);
            }

            if (__instance.hasGoldLossKeyword())
            {
                // await PlayerCmd.LoseGold(__instance.DynamicVars["Gold"].IntValue, __instance.Owner);
                PlayerCmd.LoseGold(__instance.DynamicVars["Gold"].IntValue, __instance.Owner);
            }
        }
    }
}