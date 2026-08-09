#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Rooms;
using TheBrute.Relics.Ancients;

#endregion

namespace TheBrute.Relics.Starters
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Toxemia : TheBruteRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2m)
        ];

        public override RelicModel GetUpgradeReplacement()
        {
            return ModelDb.Relic<Symbiosis>();
        }

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