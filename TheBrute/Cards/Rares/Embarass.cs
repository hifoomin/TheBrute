/*
using BaseLib.Patches.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Powers;
using static Godot.Performance;

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Embarass : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Embarass() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<EmbarassPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            /*
            var monster = cardPlay.Target.Monster;

            var enemyMove = monster.NextMove;
            List<AbstractIntent> intents = enemyMove.Intents.ToList();
            List<AbstractIntent> newIntents = [];
            foreach (AbstractIntent intent in intents)
            {
                if (intent is AttackIntent)
                {
                    AttackIntent attack = (AttackIntent)intent;

                    var baseDamage = attack.DamageCalc();
                    Main.Logger.Warn("Embarass... enemy attack intent base damage is " + baseDamage);
                    var baseHitCount = attack.Repeats;
                    Main.Logger.Warn("Embarass... enemy attack intent BASE HIT COUNT is " + baseHitCount);
                    var totalDamage = baseDamage * baseHitCount;
                    Main.Logger.Warn("Embarass... enemy attack intent FINAAL DAMAGE is " + totalDamage);
                    var newBaseDamage = Math.Floor(totalDamage / (attack.Repeats + 1));
                    Main.Logger.Warn("Embarass... enemy attack intent NEW BASE DAMAGE IS " + newBaseDamage);

                    newIntents.Add(new MultiAttackIntent((int)newBaseDamage, attack.Repeats + 1));
                }
                else
                {
                    Main.Logger.Warn("Embarass... adding attack intent");
                    newIntents.Add(intent);
                }
            }
            Main.Logger.Warn("Embarass... adding new move state");
            var newMove = new MoveState(enemyMove.StateId, enemyMove._onPerform, newIntents.ToArray());
            newMove.FollowUpState = enemyMove.FollowUpState;
            monster.SetMoveImmediate(newMove);
            */

/*
var monster = cardPlay.Target.Monster;

var enemyMove = monster.NextMove;
List<AbstractIntent> intents = enemyMove.Intents.ToList();
List<AbstractIntent> newIntents = [];
foreach (AbstractIntent intent in intents)
{
    if (intent is AttackIntent)
    {
        AttackIntent attack = (AttackIntent)intent;

        var baseDamage = attack.DamageCalc();
        Main.Logger.Warn("Embarass... enemy attack intent base damage is " + baseDamage);
        var baseHitCount = attack.Repeats;
        Main.Logger.Warn("Embarass... enemy attack intent BASE HIT COUNT is " + baseHitCount);

        newIntents.Add(new MultiAttackIntent((int)baseDamage, baseHitCount + 1));
    }
    else
    {
        Main.Logger.Warn("Embarass... adding attack intent");
        newIntents.Add(intent);
    }
}
Main.Logger.Warn("Embarass... adding new move state");
var newMove = new MoveState(enemyMove.StateId, enemyMove._onPerform, newIntents.ToArray());
newMove.FollowUpState = enemyMove.FollowUpState;
monster.SetMoveImmediate(newMove);
*/

// PowerCmd.Apply<EmbarassPower>(choiceContext, cardPlay.Target, DynamicVars["EmbarassPower"].BaseValue, Owner.Creature, this);

/*
var creatureNode = cardPlay.Target.GetCreatureNode();
if (creatureNode != null && CombatState.IsLiveCombat())
{
    TaskHelper.RunSafely(creatureNode.RefreshIntents());
}
*/
// nope just makes it go back to the original damage (haven't tested if just visually or internally too)

/*
            await PowerCmd.Apply<EmbarassPower>(choiceContext, cardPlay.Target, DynamicVars["EmbarassPower"].BaseValue, Owner.Creature, this);

            await ApplyDebuff(cardPlay.Target);
        }

        public override async Task AfterSideTurnStart(CombatSide side, IReadOnlyList<Creature> participants, ICombatState combatState)
        {
            if (side == CombatSide.Player)
            {
                foreach (var enemy in CombatManager.Instance._state.Enemies)
                {
                    var embarassCount = enemy.GetPowerAmount<EmbarassPower>();
                    for (int i = 0; i < embarassCount; i++)
                    {
                        await ApplyDebuff(enemy);
                    }
                }
            }
        }

        private async Task ApplyDebuff(Creature target)
        {
            var monster = target.Monster;
            if (monster == null)
            {
                return;
            }

            var stateId = monster.NextMove.StateId;

            var baseDamage = HolyFuckingShitSchizo.GetInitialBaseDamage(target, CombatState.RoundNumber, stateId);
            int baseHitCount = HolyFuckingShitSchizo.GetInitialHitCount(target, CombatState.RoundNumber, stateId);
            var baseTotalDamage = baseDamage * baseHitCount;

            var newHitCount = baseHitCount + Amount;
            var newDamage = Math.Ceiling(total / newHitCount);
        }
*/

/*
private async Task ApplyDebuff(Creature target)
{
    if (target != null && target.Monster != null)
    {
        var monster = target.Monster;
        var currentMove = monster.NextMove;

        var attackIntent = currentMove.Intents.OfType<AttackIntent>().FirstOrDefault();
        if (attackIntent != null)
        {
            int originalHits;
            decimal originalBaseDamage;

            originalBaseDamage = attackIntent.DamageCalc?.Invoke() ?? 0m;

            if (attackIntent is SingleAttackIntent)
            {
                originalHits = 1;
            }
            else if (attackIntent is MultiAttackIntent multi)
            {
                originalHits = multi.Repeats;
            }
            else
            {
                return;
            }

            int oldHits = originalHits;
            Main.Logger.Warn("ApplyDebuff: oldHits is " + oldHits);
            decimal oldDamage = originalBaseDamage;
            Main.Logger.Warn("ApplyDebuff: oldDamage is " + oldDamage);

            decimal oldTotal = oldDamage * oldHits;
            Main.Logger.Warn("ApplyDebuff: oldTotal is " + oldTotal);

            int newHits = oldHits + 1;
            Main.Logger.Warn("ApplyDebuff: newHits is " + newHits);

            decimal newDamage = oldTotal / newHits;
            Main.Logger.Warn("ApplyDebuff: new damage is " + newDamage);

            var newMove = new MoveState(currentMove.StateId, currentMove.PerformMove, new MultiAttackIntent((int)newDamage, newHits));
            newMove.FollowUpState = currentMove.FollowUpState;
            monster.SetMoveImmediate(newMove, true); // second param/arg was unset (so, false)
        }
    }
}
*/

/*
protected override void OnUpgrade()
{
    EnergyCost.UpgradeBy(-1);
}
*/
/*
}
}
*/