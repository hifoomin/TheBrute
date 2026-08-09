#region

using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using TheBrute.Cards.Starters;
using TheBrute.Extensions;
using TheBrute.Relics.Starters;

#endregion

namespace TheBrute.Character
{
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
            ModelDb.Card<Steal>()
        ];

        public override IReadOnlyList<RelicModel> StartingRelics =>
        [
            ModelDb.Relic<Toxemia>()
        ];

        public override CardPoolModel CardPool => ModelDb.CardPool<TheBruteCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<TheBruteRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<TheBrutePotionPool>();

        public override Control CustomIcon
        {
            get
            {
                var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
                icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
                return icon;
            }
        }

        public override string CustomIconTexturePath => "character_icon_the_brute_v2.png".CharacterUiPath();
        public override string CustomCharacterSelectIconPath => "char_select_the_brute_v2.png".CharacterUiPath();
        public override string CustomCharacterSelectLockedIconPath => "char_select_the_brute_locked_v2.png".CharacterUiPath();
        public override string CustomMapMarkerPath => "map_marker_the_brute_v2.png".CharacterUiPath();

        public override string CustomCharacterSelectBg
        {
            get
            {
                var date = DateTime.Now;

                var postfix = "_character_select_background";

                var background = SpecialEventManager.IsNewYears(date) ? $"new_years{postfix}" :
                    SpecialEventManager.IsAprilFools(date) ? $"april_fools{postfix}" :
                    SpecialEventManager.IsChristmas(date) ? $"christmas{postfix}_v2" :
                    $"default{postfix}_v2";

                return $"res://{CharacterId}/images/character/{background}.tscn";
            }
        }

        public override NCreatureVisuals CreateCustomVisuals()
        {
            return NodeFactory<NCreatureVisuals>.CreateFromScene($"res://{CharacterId}/images/character/the_brute_static_sketch_color_v2.tscn");
        }
    }
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