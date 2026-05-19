using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.ValueProps;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Embarass : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Embarass() : base(2, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<EmbarassPower>(1m)
        ];

        // CO JEST KURWA CO JEST KURWA CO JEST KURWA CO JEST KURWA CO JEST KURWA CO JEST KURWA CO JEST KURWA CO JEST KURWA
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            decimal powerAmount = 0;

            var monster = cardPlay.Target.Monster;

            var enemyMove = monster.NextMove;
            List<AbstractIntent> abstractIntents = enemyMove.Intents.ToList();
            List<AbstractIntent> newAbstractIntents = [];
            foreach (AbstractIntent abstractIntent in abstractIntents)
            {
                if (abstractIntent is AttackIntent attackIntent)
                {
                    decimal baseDamage = attackIntent.DamageCalc?.Invoke() ?? 0m;
                    Main.Logger.Warn("base damage is " + baseDamage);

                    int currentHits = attackIntent.Repeats;
                    Main.Logger.Warn("current hits is " + currentHits);
                    if (currentHits <= 0)
                    {
                        currentHits = 1;
                    }

                    decimal currentTotal = baseDamage * currentHits;
                    Main.Logger.Warn("current total is " + currentTotal);

                    int newHits = currentHits + 1;

                    Main.Logger.Warn("new hits is " + newHits);

                    decimal newBaseDamage = Math.Floor(currentTotal / newHits);
                    Main.Logger.Warn("new base damage is " + newBaseDamage);
                    newBaseDamage = Math.Max(1, newBaseDamage);
                    Main.Logger.Warn("new base damage #2 is " + newBaseDamage);

                    // var newIntent = new MultiAttackIntent((int)newBaseDamage, newHits);
                    var newIntent = new MultiAttackIntent((int)attackIntent.DamageCalc(), attackIntent.Repeats + 1);

                    powerAmount = newHits;
                }
                else
                {
                    newAbstractIntents.Add(abstractIntent);
                }
            }
            var newMove = new MoveState(enemyMove.StateId, enemyMove._onPerform, [.. newAbstractIntents])
            {
                FollowUpState = enemyMove.FollowUpState
            };
            monster.SetMoveImmediate(newMove);

            // await PowerCmd.Apply<EmbarassPower>(choiceContext, cardPlay.Target, powerAmount, Owner.Creature, this);
            await PowerCmd.Apply<EmbarassPower>(choiceContext, cardPlay.Target, DynamicVars["EmbarassPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}