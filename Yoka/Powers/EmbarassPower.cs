using BaseLib.Extensions;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Gold;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics;

namespace Yoka.Powers
{
    internal class EmbarassPower : YokaPower
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
        {
            if (attack.Attacker == null)
            {
                return hitCount;
            }

            var extraHits = attack.Attacker.GetPowerAmount<EmbarassPower>();

            if (extraHits <= 0)
            {
                return hitCount;
            }

            var damageField = AccessTools.Field(typeof(AttackCommand), "_damagePerHit");

            var damagePerHit = (decimal)damageField.GetValue(attack);

            var originalHitCount = hitCount;
            var newHitCount = hitCount + extraHits;

            var totalDamage = damagePerHit * originalHitCount;

            var newDamagePerHit = Math.Floor(totalDamage / newHitCount);

            damageField.SetValue(attack, newDamagePerHit);

            return newHitCount;
        }
    }
}