using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Potions;

namespace TheBrute.Potions
{
    internal class GoldDust : TheBrutePotion
    {
        public override PotionRarity Rarity => PotionRarity.Common;

        public override PotionUsage Usage => PotionUsage.AnyTime;

        public override TargetType TargetType => TargetType.AnyPlayer;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(15)
        ];

        protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
        {
            PotionModel.AssertValidForTargetedPotion(target);
            NCombatRoom.Instance?.PlaySplashVfx(target, new Color("f4bf57")); // hehe, bf :)
            await PlayerCmd.GainGold(base.DynamicVars.Gold.BaseValue, target.Player);

            VfxCmd.PlayOnCreatureCenter(target, "vfx/vfx_coin_explosion_regular");
        }
    }
}