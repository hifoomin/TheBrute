#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Anathema : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Anathema() : base(9, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(33, ValueProp.Move),
            new EnergyVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var result = await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState!).Execute(choiceContext);

            AudioUtils.PlayAoeSlash(result);
        }

        public override Task AfterCardEnteredCombat(CardModel card)
        {
            if (card != this)
            {
                return Task.CompletedTask;
            }
            if (IsClone)
            {
                return Task.CompletedTask;
            }

            Main.Logger.Warn("anathema after card entered combat, reducing cost");

            var amount = (int)ThornsTracker.timesThornsGainedThisCombat[Owner.Creature];
            EnergyCost.AddThisCombat(-amount * DynamicVars.Energy.IntValue);
            return Task.CompletedTask;
        }

        // part of this card is inside thornstracker

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}