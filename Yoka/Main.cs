using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;

namespace Yoka;

[ModInitializer(nameof(Awake))]
public partial class Main : Node
{
    public const string ModId = "Yoka"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    //
    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Awake()
    {
        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }
}