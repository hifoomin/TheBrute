using BaseLib.Abstracts;
using BaseLib.Extensions;
using Yoka.Extensions;
using Godot;

namespace Yoka.Powers;

public abstract class YokaPower : CustomPowerModel
{
    //Loads from CharMod/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}