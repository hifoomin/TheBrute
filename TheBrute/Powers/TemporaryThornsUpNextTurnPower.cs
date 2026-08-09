#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Powers
{
#pragma warning disable STS001 // Symbol missing localization

    internal class TemporaryThornsUpNextTurnPower : TheBrutePower
#pragma warning restore STS001 // Symbol missing localization
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
        }

        public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner) && AmountOnTurnStart != 0)
            {
                Flash();
                await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
                await PowerCmd.Apply<TemporaryThornsUpPower>(new ThrowingPlayerChoiceContext(), Owner, Amount, Owner, null);
                await PowerCmd.Remove(this);
            }
        }
    }
}