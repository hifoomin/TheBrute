#region

using MegaCrit.Sts2.Core.Entities.Players;

#endregion

namespace TheBrute.Cards
{
    public static class Utils
    {
        public static bool HasGold(Player owner, int amount)
        {
            return owner.Gold >= amount;
        }
    }
}