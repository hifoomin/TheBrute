#region

using System.Runtime.InteropServices;
using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Vfx;
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

        public static readonly Color Color = new("C7FF00");

        public override CharacterGender Gender => CharacterGender.Feminine;

        // public override int StartingHp => 70;
        public override int StartingHp => 76;
        public override int StartingGold => 99;
        public override Color NameColor => new("C7FF00");
        public override Color EnergyLabelOutlineColor => new("3F4F00FF");
        public override Color DialogueColor => new("3C411A");
        public override VfxColor SpeechBubbleColor => VfxColor.Swamp;
        public override Color MapDrawingColor => new("686317");
        public override Color RemoteTargetingLineColor => new("93CA02FF");
        public override Color RemoteTargetingLineOutline => new("3F4F00FF");

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
                var date = DateTime.Today;

                var postfix = "_character_select_background";

                var background = SpecialEventManager.IsNewYears(date) ? $"new_years{postfix}" :
                    SpecialEventManager.IsAprilFools(date) ? $"april_fools{postfix}" :
                    SpecialEventManager.IsChristmas(date) ? $"christmas{postfix}_v2" : $"default{postfix}_v2";

                return $"res://{CharacterId}/images/character/{background}.tscn";
            }
        }

        public override string CharacterTransitionSfx => $"{Main.AudioPath}character_transition.ogg";
        public override string CharacterSelectSfx => $"{Main.AudioPath}character_select.ogg";

        // public override string CustomAttackSfx => "event:/sfx/characters/silent/silent_attack";
        // alright candidate
        public override string CustomAttackSfx => "event:/sfx/enemy/enemy_attacks/obscura/obscura_attack";
        // keeping this for now, sounds decent

        public override NCreatureVisuals CreateCustomVisuals()
        {
            return NodeFactory<NCreatureVisuals>.CreateFromScene($"res://{CharacterId}/images/character/the_brute_static_sketch_color_v2.tscn");
        }

        public override List<string> GetArchitectAttackVfx()
        {
            var num = 5;
            var list = new List<string>(num);
            CollectionsMarshal.SetCount(list, num);
            var span = CollectionsMarshal.AsSpan(list);
            var num2 = 0;
            span[num2] = "vfx/vfx_attack_blunt";
            num2++;
            span[num2] = "vfx/vfx_heavy_blunt";
            num2++;
            span[num2] = "vfx/vfx_attack_slash";
            num2++;
            span[num2] = "vfx/vfx_bloody_impact";
            num2++;
            span[num2] = "vfx/vfx_rock_shatter";
            return list;
        }
    }
}