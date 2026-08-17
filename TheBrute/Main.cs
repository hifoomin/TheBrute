#region

using BaseLib.Audio;
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

#endregion

namespace TheBrute
{
    [ModInitializer(nameof(Awake))]
    public partial class Main : Node
    {
        public const string ModId = "TheBrute"; //Used for resource filepath
        public const string ResPath = $"res://{ModId}";
        public const string AudioPath = $"res://{ModId}/audio/";

        public static bool isAprilFools = false;

        public static readonly AutoModAudio Audio = new($"res://{ModId}/audio");

        //
        public static Logger Logger { get; } = new(ModId, LogType.Generic);

        public static void Awake()
        {
            ModConfigRegistry.Register("TheBrute", new Config());
            Harmony harmony = new(ModId);

            harmony.PatchAll();
        }
    }
}