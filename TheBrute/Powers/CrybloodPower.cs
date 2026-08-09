#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Powers
{
    internal class CrybloodPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        public override int DisplayAmount => Owner.MaxHp * Amount / 100;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner.Player != player)
            {
                return;
            }

            Flash();

            await CreatureCmd.GainBlock(Owner, DisplayAmount, ValueProp.Unpowered, null);
        }
    }
}