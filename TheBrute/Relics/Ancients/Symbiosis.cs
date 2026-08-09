#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Rooms;

#endregion

namespace TheBrute.Relics.Ancients
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Symbiosis : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(4m)
        ];

        public override async Task AfterCombatVictory(CombatRoom _)
        {
            if (!Owner.Creature.IsDead)
            {
                Flash();
                await CreatureCmd.GainMaxHp(Owner.Creature, DynamicVars.MaxHp.BaseValue);
            }
        }
    }
}