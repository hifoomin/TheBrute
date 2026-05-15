using MegaCrit.Sts2.Core.Commands;
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

namespace Yoka.Relics.Starters
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Toxemia : YokaRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<ThornsPower>(2m), new HealVar(4m)];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ThornsPower>()];

        public override RelicModel GetUpgradeReplacement()
        {
            return ModelDb.Relic<Ancients.Symbiosis>();
        }

        public override async Task AfterRoomEntered(AbstractRoom room)
        {
            if (room is CombatRoom)
            {
                Flash();
                await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, null);
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