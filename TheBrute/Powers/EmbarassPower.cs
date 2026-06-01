/*
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
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
using TheBrute.Relics;

namespace TheBrute.Powers
{
    internal class EmbarassPower : TheBrutePower
    {
        public override PowerType Type => PowerType.Debuff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
        {
            if (dealer != Owner)
            {
                // Main.Logger.Warn("co jest kurwa");
                return 1m;
            }

            Main.Logger.Warn("ModifyDamageMultiplicative: damage before changes is " + amount);

            decimal result = 1m;

            for (int i = 0; i < Amount; i++)
            {
                result *= 0.5m;
            }

            Main.Logger.Warn("ModifyDamageMultiplicative: damage after changes is " + result);
            Main.Logger.Warn("ModifyDamageMultiplicative: damage after changes AND CEILING is " + Math.Ceiling(result));

            return Math.Ceiling(result);

            /*

            var monster = Owner.Monster;
            var currentMove = monster.NextMove;

            var attackIntent = currentMove.Intents.OfType<AttackIntent>().FirstOrDefault();

            Main.Logger.Warn("EMBARASS POWER     , , , attack intent hit count is " + attackIntent.Repeats);

            int originalHits = attackIntent.Repeats - 1;
            Main.Logger.Warn("EMBARASS POWER     , , , OROGINAL HITS is " + originalHits);
            decimal originalDamage = (amount * 2);
            Main.Logger.Warn("EMBARASS POWER     , , , OROGINAL DAMAGEEEEEE is " + originalDamage);

            int newHits = originalHits + Amount;

            Main.Logger.Warn("EMBARASS POWER     , , , NEW HITS AMOUNT is " + newHits);

            decimal originalTotal = originalHits * originalDamage;

            Main.Logger.Warn("EMBARASS POWER     , , , ORIGINAL FINAAAAAAAAAL DAMAGE is " + originalTotal);

            decimal newDamage = originalTotal / newHits;

            Main.Logger.Warn("EMBARASS POWER     , , , WE ARE RETURNING is " + (newDamage / amount));

            return newDamage / amount;

            */
// }

/*

    public override int ModifyAttackHitCount(AttackCommand attack, int hitCount)
    {
        Main.Logger.Warn("ModifyAttackHitCount: hit count before changes is " + hitCount);
        if (attack.Attacker.IsEnemy && attack.DamageProps.IsPoweredAttack())
        {
            Main.Logger.Warn("ModifyAttackHitCount: hit count after changes is " + hitCount);
            return hitCount + Amount;
        }
        return hitCount;
    }
}
}
*/