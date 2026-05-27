using BaseLib.Abstracts;
using TheBrute.Extensions;
using Godot;

namespace TheBrute.Character;

public class TheBrutePotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => TheBrute.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}