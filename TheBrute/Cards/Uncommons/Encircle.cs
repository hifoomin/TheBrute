#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Uncommons
{
    internal class Encircle : TheBruteCard
    {
        public Encircle() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<EncirclePower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<EncirclePower>(choiceContext, Owner.Creature, DynamicVars["EncirclePower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["EncirclePower"].UpgradeValueBy(1m);
        }
    }
}