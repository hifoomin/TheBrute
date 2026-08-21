#region

using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Cards.Commons;
using TheBrute.Extensions;

#endregion

namespace TheBrute.Powers
{
    public class FrustrationPower : CustomTemporaryPowerModel
    {
        protected override Func<PlayerChoiceContext, Creature, decimal, Creature?, CardModel?, bool, Task> ApplyPowerFunc => PowerCmd.Apply<StrengthPower>;

        public override PowerType Type => PowerType.Buff;

        public override PowerModel InternallyAppliedPower => ModelDb.Power<StrengthPower>();
        public override AbstractModel OriginModel => ModelDb.Card<Frustration>();
        public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
        public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();

        protected override bool InvertInternalPowerAmount => false;
    }
}