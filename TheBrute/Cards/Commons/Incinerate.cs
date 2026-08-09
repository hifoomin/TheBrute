#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Commons
{
    internal class Incinerate : TheBruteCard
    {
        public Incinerate() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(14m, ValueProp.Move),
            new GoldVar(3)
        ];

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(this, cardPlay).Targeting(cardPlay.Target)
                    .WithHitFx( /*"vfx/vfx_attack_slash"*/)
                    .Execute(choiceContext);

                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");

                await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m);
        }
    }
}