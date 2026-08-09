#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Loan : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Loan() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override bool ShouldGlowRedInternal => !Utils.HasGold(Owner, DynamicVars.Gold.IntValue);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(16),
            new PowerVar<BufferPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.HasGold(Owner, DynamicVars.Gold.IntValue))
            {
                await PowerCmd.Apply<BufferPower>(choiceContext, Owner.Creature, DynamicVars["BufferPower"].BaseValue, Owner.Creature, this);

                VfxCmd.PlayOnCreatureCenter(Owner.Creature, "vfx/vfx_coin_explosion_regular");

                await PlayerCmd.LoseGold(DynamicVars.Gold.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Gold.UpgradeValueBy(-4m);
        }
    }
}