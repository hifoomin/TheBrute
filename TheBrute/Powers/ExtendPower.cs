using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Powers
{
    internal class ExtendPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
           HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<ThornsPower>(0m),
            new PowerVar<TemporaryThornsUpPower>(0m)
        ];

        public void SetTemporaryThornsUpAmount(decimal amount)
        {
            AssertMutable();
            DynamicVars["ThornsPower"].BaseValue = amount;
            DynamicVars["TemporaryThornsUpPower"].BaseValue = amount;
        }

        public override async Task AfterSideTurnStartLate(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (participants.Contains(Owner))
            {
                Flash();
                await PowerCmd.Apply<ThornsPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars["ThornsPower"].BaseValue, Owner, null);
                await PowerCmd.Apply<TemporaryThornsUpPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars["TemporaryThornsUpPower"].BaseValue, Owner, null);
                await PowerCmd.Decrement(this);
            }
        }
    }
}