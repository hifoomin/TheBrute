using BaseLib.Abstracts;
using BaseLib.Extensions;
using TheBrute.Extensions;
using Godot;

namespace TheBrute.Powers;

public abstract class TheBrutePower : CustomPowerModel
{
    //Loads from CharMod/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();

    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}