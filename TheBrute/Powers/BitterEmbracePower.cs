#region

using MegaCrit.Sts2.Core.Entities.Powers;

#endregion

namespace TheBrute.Powers
{
    internal class BitterEmbracePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;
    }
}