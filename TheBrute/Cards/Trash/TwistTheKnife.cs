#region

using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using TheBrute.Character;

#endregion

namespace TheBrute.Cards.Trash
{
    [Pool(typeof(EventCardPool))]
    internal class TwistTheKnife : TheBruteCard
    {
        public TwistTheKnife() : base(1, CardType.Attack, CardRarity.Event, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            EnergyHoverTip
        ];

        public override CardPoolModel VisualCardPool => ModelDb.CardPool<TheBruteCardPool>();

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(1m, ValueProp.Move),
            new CalculationBaseVar(0m),
            new ExtraDamageVar(1m),
            new CalculatedDamageVar(ValueProp.Move).WithMultiplier(delegate(CardModel card, Creature? _)
            {
                if (card.Pile == null)
                {
                    return 0m;
                }

                var energySpentThisCombat = (from e in CombatManager.Instance.History.Entries.OfType<EnergySpentEntry>()
                                             where /*e.HappenedThisTurn(card.CombatState) &&*/ e.Actor.Player == card.Owner
                                             select e).Sum(c => c.Amount);

                // Main.Logger.Warn("energy spent this combat is equal to " + energySpentThisCombat);

                if (card.Pile.Type == PileType.Play)
                {
                    energySpentThisCombat -= card.EnergyCost.GetWithModifiers(CostModifiers.All);
                    // Main.Logger.Warn("card piletype is play, subtracting its energy from energy spent this combat, current value: " + energySpentThisCombat);
                }
                // Main.Logger.Warn("final value of energy spent this combat: " + energySpentThisCombat);
                return energySpentThisCombat;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await DamageCmd.Attack(DynamicVars.CalculatedDamage).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_blunt", null, "blunt_attack.mp3")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}