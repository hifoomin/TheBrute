using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;
using Yoka.Relics;

namespace Yoka.Relics.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class ThornyHelmet : YokaRelic
#pragma warning restore STS001 // Symbol missing localization
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        private bool _usedThisCombat;

        private bool UsedThisCombat
        {
            get
            {
                return _usedThisCombat;
            }
            set
            {
                AssertMutable();
                _usedThisCombat = value;
            }
        }

        // stupid shit because of their garbage systems lol

        public override bool TryModifyPowerAmountReceived(PowerModel canonicalPower, Creature target, decimal amount, Creature? applier, out decimal modifiedAmount)
        {
            modifiedAmount = amount;
            if (canonicalPower is not ThornsPower || canonicalPower is not TemporaryThornsPower || canonicalPower is not TemporaryThornsNextTurnPower)
            {
                return false;
            }
            if (target != base.Owner.Creature)
            {
                return false;
            }
            if (amount <= 0m)
            {
                return false;
            }
            if (UsedThisCombat)
            {
                return false;
            }
            modifiedAmount *= 2m;
            return true;
        }

        public override Task AfterModifyingPowerAmountReceived(PowerModel power)
        {
            Flash();
            UsedThisCombat = true;
            return Task.CompletedTask;
        }

        public override Task AfterCombatEnd(CombatRoom _)
        {
            UsedThisCombat = false;
            return Task.CompletedTask;
        }
    }
}