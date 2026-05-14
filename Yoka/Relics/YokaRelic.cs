using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Yoka.Character;
using Yoka.Extensions;
using Godot;

namespace Yoka.Relics;

[Pool(typeof(YokaRelicPool))]
public abstract class YokaRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();
    protected override string PackedIconOutlinePath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();
    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}