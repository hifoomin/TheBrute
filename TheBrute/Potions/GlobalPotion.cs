using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models.PotionPools;
using TheBrute.Character;
using TheBrute.Extensions;

namespace TheBrute.Potions;

[Pool(typeof(SharedPotionPool))]
public abstract class GlobalPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
}