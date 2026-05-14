using BaseLib.Abstracts;
using Yoka.Extensions;
using Godot;

namespace Yoka.Character;

public class YokaRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Yoka.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}