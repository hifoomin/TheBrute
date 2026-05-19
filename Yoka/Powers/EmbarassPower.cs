using BaseLib.Extensions;
using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
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
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Relics;

namespace Yoka.Powers
{
    // mostly copied from Eka/yikestile, thank you!!!
    internal class EmbarassPower : YokaPower
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        /*
        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (dealer != Owner)
            {
                return 1m;
            }

            var monster = Owner.Monster;
            var currentMove = monster.NextMove;

            var attackIntent = currentMove.Intents.OfType<AttackIntent>().FirstOrDefault();

            int originalHits = attackIntent.Repeats - 1;
            decimal originalDamage = amount;

            int newHits = originalHits + Amount;

            decimal originalTotal = originalHits * originalDamage;

            decimal newDamage = originalTotal / newHits;

            return newDamage / amount;
        }
        */

        //public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
        //{
        //    if (attack.Attacker.IsEnemy /*&& attack.DamageProps.IsPoweredAttack()*/)
        //    {
        //        Main.Logger.Warn("setting hit count to " + (hitCount + Amount));
        //        return hitCount + Amount;
        //    }
        //    return hitCount;
        //}
    }
}