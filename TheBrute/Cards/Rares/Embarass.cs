/*
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
using TheBrute.Powers;

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
                    var baseHitCount = attack.Repeats;
                    var totalDamage = baseDamage * baseHitCount;
                    var newBaseDamage = Math.Floor(totalDamage / (attack.Repeats + 1));

                    newIntents.Add(new MultiAttackIntent((int)newBaseDamage, attack.Repeats + 1));
                }
                else
                {
                    newIntents.Add(intent);
                }
            }
            var newMove = new MoveState(enemyMove.StateId, enemyMove._onPerform, newIntents.ToArray());
            newMove.FollowUpState = enemyMove.FollowUpState;
            monster.SetMoveImmediate(newMove);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}
*/