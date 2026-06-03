using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using TheBrute.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using TheBrute.Cards.Starters;
using TheBrute.Relics;
using TheBrute.Relics.Starters;
using HarmonyLib;
using MegaCrit.Sts2.Core.Localization;

namespace TheBrute.Character;

#pragma warning disable STS001 // Symbol missing localization

public class TheBrute : PlaceholderCharacterModel
#pragma warning restore STS001 // Symbol missing localization
{
    public const string CharacterId = "TheBrute";

    public static readonly Color Color = new("726c10");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 70;

    public override Color MapDrawingColor => Color;

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
        ModelDb.Card<Steal>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<Toxemia>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<TheBruteCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheBruteRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheBrutePotionPool>();

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

    public override string CustomIconTexturePath => "character_icon_the_brute.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_the_brute.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_the_brute_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_the_brute.png".CharacterUiPath();
    // public override string CustomCharacterSelectBg => "res://character_select.tscn";
}

/*
[HarmonyPatch(typeof(CharacterModel), "AddDetailsTo")]
internal class WhatTheFuck
{
    public static void Postfix(CharacterModel __instance, LocString locString)
    {
        Main.Logger.Warn("base.Id.Entry is " + __instance.Id.Entry);
    }
}
*/

// HAHAJEKIFDASJJKLFSKADJFGAHK OF COURSE I CANT EVEN FUCKING WRITE A HARMONY APATCH TO SEE WHAT THE FUCK IS WRONG WITH COLORFUL PHILOSOPHERS FUCKING GARBAGE GAME WITH ZERO USEFUL LOGGING