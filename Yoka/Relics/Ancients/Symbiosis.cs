using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;
using Yoka.Relics;

namespace Yoka.Relics.Ancients
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Symbiosis : YokaRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<SymbiosisPower>(1m), new HealVar(7m)
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            if (room is CombatRoom)
            {
                Flash();
                await PowerCmd.Apply<SymbiosisPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["SymbiosisPower"].BaseValue, Owner.Creature, null);
            }
        }

        public override async Task AfterCombatVictory(CombatRoom _)
        {
            if (!Owner.Creature.IsDead)
            {
                Flash();
                await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
            }
        }
    }
}