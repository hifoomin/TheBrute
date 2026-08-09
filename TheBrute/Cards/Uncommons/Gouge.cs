#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Gouge : TheBruteCard
    {
        public Gouge() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.Static(StaticHoverTip.Block)
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            // new MaxHpVar(1m),
            new DamageVar(3m, ValueProp.Move),
            new RepeatVar(3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var hittableEnemies = CombatState.HittableEnemies;

            // await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            foreach (var hittableEnemy in hittableEnemies)
            {
                await CreatureCmd.LoseBlock(choiceContext, hittableEnemy, hittableEnemy.Block, Owner.Creature);
            }

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).TargetingAllOpponents(CombatState).WithHitCount(DynamicVars.Repeat.IntValue)
                .WithHitFx("vfx/vfx_giant_horizontal_slash", null, "slash_attack.mp3")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}