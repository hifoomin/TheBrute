#region

using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class ReducedMaximumHandSizePower : TheBrutePower, IMaxHandSizeModifier
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public int ModifyMaxHandSize(Player player, int currentMaxHandSize)
        {
            if (player == Owner.Player)
            {
                Flash();
                return currentMaxHandSize - Amount;
            }
            return currentMaxHandSize;
        }
    }
}