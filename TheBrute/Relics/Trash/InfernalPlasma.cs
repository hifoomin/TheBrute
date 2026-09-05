#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Relics.Trash
{
    [Pool(typeof(EventRelicPool))]
    internal class InfernalPlasma : TheBruteRelic
    {
        private bool _usedThisEvent;
        public override RelicRarity Rarity => RelicRarity.Event;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(3m),
            new("KurwaTrashHeap", 0m)
        ];

        [SavedProperty]
        private bool UsedThisEvent
        {
            get => _usedThisEvent;
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