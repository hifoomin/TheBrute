#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Powers
{
    internal class ComboPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
        {
            if (card.Owner.Creature != Owner)
            {
                return playCount;
            }
            if (card.Type != CardType.Attack)
            {
                return playCount;
            }
            return playCount + Amount;
        }

        public override async Task AfterModifyingCardPlayCount(CardModel card)
        {
            await PowerCmd.Remove(this);
        }
    }
}