#region

using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards
{
    public static class Keywords
    {
        [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
        public static CardKeyword transmutedKeyword;

        [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
        public static CardKeyword goldLossKeyword;

        public static bool hasTransmutedKeyword(this CardModel card)
        {
            return card.Keywords.Contains(transmutedKeyword);
        }

        public static bool hasGoldLossKeyword(this CardModel card)
        {
            return card.Keywords.Contains(goldLossKeyword);
        }
    }

    [HarmonyPatch(typeof(CardModel), "OnPlayWrapper")]
    public class KeywordsOnPlayWrapperPatch
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
                PlayerCmd.LoseGold(__instance.DynamicVars.Gold.IntValue, __instance.Owner);
            }
        }
    }
}