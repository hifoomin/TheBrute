#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Powers
{
    internal class HunkerDownPower : TheBrutePower
    {
        private bool shouldRun = true;
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel? cardSource)
        {
            if (creature == Owner && amount > 0 && shouldRun)
            {
                Flash();
                await CardPileCmd.Draw(new ThrowingPlayerChoiceContext(), Amount, Owner.Player);
            }
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            shouldRun = true;
        }

        public override async Task BeforeSideTurnEndVeryEarly(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
        {
            shouldRun = false;
        }
    }
}