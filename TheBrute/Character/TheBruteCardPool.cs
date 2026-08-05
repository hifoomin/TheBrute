using BaseLib.Abstracts;
using TheBrute.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;

namespace TheBrute.Character;

public class TheBruteCardPool : CustomCardPoolModel
{
    public override string Title => TheBrute.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy_v3.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy_v3.png".ImagePath();

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 1f; //Hue; changes the color. // 1.1
    public override float S => 1f; //Saturation // 0.82
    public override float V => 1f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    public override Texture2D CustomFrame(CustomCardModel card)
    {
        var attackFrame = PreloadManager.Cache.GetTexture2D("cards/attackframe_v2.png".ImagePath());
        var defaultFrame = PreloadManager.Cache.GetTexture2D("cards/skillframe_v2.png".ImagePath());
        var powerFrame = PreloadManager.Cache.GetTexture2D("cards/powerframe_v2.png".ImagePath());

        return card.Type switch
        {
            CardType.Attack => attackFrame,
            CardType.Skill => defaultFrame,
            CardType.Power => powerFrame,
            CardType.Curse => defaultFrame,
            CardType.Status => defaultFrame,
            CardType.Quest => defaultFrame,
            CardType.None => attackFrame,
            _ => defaultFrame
        };
    }

    //Color of small card icons
    public override Color DeckEntryCardColor => new("a3973e");

    public override bool IsColorless => false;
}