using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheBrute.Character;
using TheBrute.Extensions;

namespace TheBrute.Potions;

[Pool(typeof(TheBrutePotionPool))]
public abstract class TheBrutePotion : CustomPotionModel
{
    public override string? CustomPackedImagePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();
}