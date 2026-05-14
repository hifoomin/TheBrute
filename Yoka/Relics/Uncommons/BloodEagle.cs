using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics;

namespace Yoka.Relics.Uncommons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class BloodEagle : YokaRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new HealVar(1m)
        ];
    }

    [HarmonyPatch(typeof(MegaCrit.Sts2.Core.Commands.PlayerCmd), "LoseGold")]
    internal class LoseGoldPatch
    {
        private static void Postfix(Task __result, Player player, GoldLossType goldLossType)
        {
            var bloodEagle = player.GetRelic<BloodEagle>();

            if (bloodEagle != null)
            {
                bloodEagle.Flash();
                player.Creature.HealInternal(bloodEagle.DynamicVars.Heal.BaseValue);
            }
        }
    }
}