using BaseLib.Patches.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.MonsterMoves.Intents;
using MegaCrit.Sts2.Core.MonsterMoves.MonsterMoveStateMachine;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.ValueProps;
using System.Reflection;
using TheBrute.Powers;
using static Godot.Performance;

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Embarass : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Embarass() : base(2, CardType.Skill, CardRarity.Rare, TargetTypes.MoreThanOneBaseDamageAttackIntent)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<WeakPower>(),
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<WeakPower>(1m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            await PowerCmd.Apply<WeakPower>(choiceContext, cardPlay.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);

            if (cardPlay.Target?.Monster is MonsterModel monster)
            {
                var enemyMove = monster.NextMove;
                if (enemyMove == null) return;

                List<AbstractIntent> newIntents = [];
                bool hasAttackIntent = false;

                foreach (var intent in enemyMove.Intents)
                {
                    if (intent is AttackIntent attack)
                    {
                        hasAttackIntent = true;
                        int originalHits = (attack is MultiAttackIntent multi) ? multi.Repeats : 1;

                        decimal originalModifiedDamagePerHit = attack.GetSingleDamage([Owner.Creature], monster.Creature);
                        decimal totalOriginalModifiedDamage = originalModifiedDamagePerHit * originalHits;

                        int newHits = originalHits * 2;
                        decimal strengthAmount = monster.Creature.GetPower<StrengthPower>()?.Amount ?? 0m;

                        decimal desiredModifiedDamagePerNewHit = totalOriginalModifiedDamage / newHits;
                        decimal baseDamageForNewIntent = desiredModifiedDamagePerNewHit - strengthAmount;

                        int finalBaseDamageForNewIntent = Math.Max(0, (int)baseDamageForNewIntent);

                        if (desiredModifiedDamagePerNewHit > 0 && finalBaseDamageForNewIntent <= 0)
                        {
                            finalBaseDamageForNewIntent = 1;
                        }

                        newIntents.Add(new MultiAttackIntent(finalBaseDamageForNewIntent, newHits));
                    }
                    else
                    {
                        newIntents.Add(intent);
                    }
                }

                if (hasAttackIntent)
                {
                    var moveType = typeof(MoveState);

                    var actionField = moveType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                              .FirstOrDefault(f => f.FieldType == typeof(Func<IReadOnlyList<Creature>, Task>));

                    var intentsField = moveType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                               .FirstOrDefault(f => typeof(IEnumerable<AbstractIntent>).IsAssignableFrom(f.FieldType));

                    Func<IReadOnlyList<Creature>, Task>? originalOnPerform = null;
                    if (actionField != null)
                    {
                        originalOnPerform = actionField.GetValue(enemyMove) as Func<IReadOnlyList<Creature>, Task>;
                    }

                    Func<IReadOnlyList<Creature>, Task> wrappedOnPerform = async (targets) =>
                    {
                        EmbarassTracker.ModifiedCreatures.Add(monster.Creature);
                        try
                        {
                            if (originalOnPerform != null)
                            {
                                await originalOnPerform(targets);
                            }
                        }
                        finally
                        {
                            EmbarassTracker.ModifiedCreatures.Remove(monster.Creature);
                        }
                    };

                    actionField?.SetValue(enemyMove, wrappedOnPerform);

                    if (intentsField != null)
                    {
                        if (intentsField.FieldType == typeof(AbstractIntent[]))
                        {
                            intentsField.SetValue(enemyMove, newIntents.ToArray());
                        }
                        else if (intentsField.FieldType == typeof(List<AbstractIntent>))
                        {
                            intentsField.SetValue(enemyMove, newIntents);
                        }
                        else if (intentsField.FieldType == typeof(IReadOnlyList<AbstractIntent>))
                        {
                            intentsField.SetValue(enemyMove, newIntents.AsReadOnly());
                        }
                    }

                    monster.SetMoveImmediate(enemyMove);
                }
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}