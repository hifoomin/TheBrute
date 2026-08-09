#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Racemization : TheBruteCard
    {
        public Racemization() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<DexterityPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(30m, ValueProp.Move),
            new PowerVar<DexterityPower>(2m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<DexterityPower>(choiceContext, Owner.Creature, -DynamicVars.Dexterity.BaseValue, Owner.Creature, this);

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(6m);
        }
    }
}