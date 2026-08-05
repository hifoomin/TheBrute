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
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBrute.Relics;

namespace TheBrute.Relics.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class InfernalPlasma : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(3m)
        ];

        private bool _usedThisEvent;

        [SavedProperty]
        private bool UsedThisEvent
        {
            get
            {
                return _usedThisEvent;
            }
            set
            {
                AssertMutable();
                _usedThisEvent = value;
            }
        }

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            if (!Owner.Creature.IsDead)
            {
                if (room != null && room.RoomType == RoomType.Event)
                {
                    UsedThisEvent = false;
                }
                else
                {
                    UsedThisEvent = true;
                }
            }
        }

        public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (Owner.RunState.CurrentRoom != null && Owner.RunState.CurrentRoom.RoomType == RoomType.Event && target == Owner.Creature && !UsedThisEvent)
            {
                Flash();
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
                UsedThisEvent = true;
            }
        }
    }
}