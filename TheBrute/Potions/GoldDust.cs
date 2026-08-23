#region

using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Merchant;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;

#endregion

namespace TheBrute.Potions
{
    internal class GoldDust : TheBrutePotion
    {
        public override PotionRarity Rarity => PotionRarity.Common;

        public override PotionUsage Usage => PotionUsage.AnyTime;

        public override TargetType TargetType => TargetType.AnyPlayer;

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(15)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            AssertValidForTargetedPotion(target);

            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("f4bf57")); // hehe, bf :)
            await PlayerCmd.GainGold(DynamicVars.Gold.BaseValue, target.Player);

            VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_coin_explosion_regular");
        }
    }

    [HarmonyPatch(typeof(MerchantPotionEntry))]
    internal class GoldDustMerchantPotionEntryPatch
    {
        [HarmonyPatch(MethodType.Constructor, typeof(PotionModel), typeof(Player)), HarmonyPostfix]
        private static void Postfix(MerchantPotionEntry __instance)
        {
            if (__instance.Model?.Id == ModelDb.Potion<GoldDust>().Id)
            {
                __instance.Model = PotionFactory.CreateRandomPotionOutOfCombat(__instance._player, __instance._player.PlayerRng.Shops, [
                ]).ToMutable();

                __instance.CalcCost();
                SaveManager.Instance.MarkPotionAsSeen(__instance.Model);
            }
        }
    }
}