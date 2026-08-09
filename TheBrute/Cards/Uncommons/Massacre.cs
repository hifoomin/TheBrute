#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Massacre : TheBruteCard
    {
        public Massacre() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override bool ShouldGlowGoldInternal => GoldTracker.GetChangedGoldThisTurn(Owner.Creature);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(13m, ValueProp.Move),
            new RepeatVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var hitCount = GoldTracker.GetChangedGoldThisTurn(Owner.Creature) ? DynamicVars.Repeat.IntValue : 1;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithHitCount(hitCount)
                .WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}