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
using Yoka.Relics;

namespace Yoka
{
    public static class Keywords
    {
        [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
        public static CardKeyword hpLossKeyword;

        public static bool hasHpLossKeyword(this CardModel card)
        {
            return card.Keywords.Contains(hpLossKeyword);
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

            if (__instance.hasHpLossKeyword())
            {
                CreatureCmd.Damage(choiceContext, __instance.Owner.Creature, __instance.DynamicVars.HpLoss.BaseValue, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, __instance);
            }

            if (__instance.hasGoldLossKeyword())
            {
                PlayerCmd.LoseGold(__instance.DynamicVars["Gold"].IntValue, __instance.Owner);
            }
        }
    }
}