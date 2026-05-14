using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Yoka.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using Yoka.Cards.Starters;
using Yoka.Relics;
using Yoka.Relics.Starters;

namespace Yoka.Character;

#pragma warning disable STS001 // Symbol missing localization

public class Yoka : PlaceholderCharacterModel
#pragma warning restore STS001 // Symbol missing localization
{
    public const string CharacterId = "Yoka";

    public static readonly Color Color = new("ffffff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 84;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        // 4x strike
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),
        ModelDb.Card<Strike>(),

        // 4x defend
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),
        ModelDb.Card<Defend>(),

        // 2x starter
        ModelDb.Card<Bristle>(),
        ModelDb.Card<KarmaStrike>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Toxemia>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<YokaCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<YokaRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<YokaPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */

    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    // public override string CustomCharacterSelectBg => "res://character_select.tscn";
}