#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using TheBrute.Powers;

#endregion

namespace TheBrute.Relics.Rares
{
    internal class SpineCharm : TheBruteRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1)
        ];

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner.Creature) && Owner.PlayerCombatState!.TurnNumber <= 1)
            {
                await PowerCmd.Apply<SpineCharmPower>(new ThrowingPlayerChoiceContext(), Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, null);
            }
        }
    }
}