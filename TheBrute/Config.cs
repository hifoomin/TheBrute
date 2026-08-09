#region

using BaseLib.Config;

#endregion

namespace TheBrute
{
    internal class Config : SimpleModConfig
    {
        public static bool EnableTrashHeapAdditions { get; set; } = true;
        public static bool EnableColorfulPhilosophersAdditions { get; set; } = true;
    }
}