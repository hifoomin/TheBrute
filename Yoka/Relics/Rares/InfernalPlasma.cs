using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics;

namespace Yoka.Relics.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class InfernalPlasma : YokaRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m)
        ];

        private bool shouldRun = false;

        public override async Task AfterRoomEntered(AbstractRoom _)
        {
            if (!Owner.Creature.IsDead)
            {
                if (Owner.RunState.CurrentRoom.RoomType == RoomType.Event)
                {
                    shouldRun = true;
                }
            }
        }

        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (shouldRun)
            {
                Flash();
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
                shouldRun = false;
            }
        }
    }
}