using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using TheBrute.Character;
using TheBrute.Extensions;
using Godot;

namespace TheBrute.Relics;

[Pool(typeof(TheBruteRelicPool))]
public abstract class TheBruteRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}