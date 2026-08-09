#region

using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.PotionPools;
using TheBrute.Extensions;

#endregion

namespace TheBrute.Potions
{
    [Pool(typeof(SharedPotionPool))]
    public abstract class GlobalPotion : CustomPotionModel
    {
        public override string? CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
    }
}