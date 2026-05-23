using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
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

namespace Yoka.Powers
{
    internal class ExtendPower : YokaPower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override PowerInstanceType InstanceType => PowerInstanceType.Instanced;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<TemporaryThornsPower>(0m)
        ];

        public void SetTemporaryThornsAmount(decimal amount)
        {
            AssertMutable();
            DynamicVars["TemporaryThornsPower"].BaseValue = amount;
        }

        public override async Task AfterEnergyReset(Player player)
        {
            if (player == Owner.Player)
            {
                await PowerCmd.Apply<TemporaryThornsPower>(new ThrowingPlayerChoiceContext(), Owner, DynamicVars["TemporaryThornsPower"].BaseValue, Owner, null);
                await PowerCmd.Decrement(this);
            }
        }
    }
}