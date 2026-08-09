#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class RazorForm : TheBruteCard
    {
        public RazorForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<RazorFormPower>(1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await PowerCmd.Apply<RazorFormPower>(choiceContext, Owner.Creature, DynamicVars["RazorFormPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["RazorFormPower"].UpgradeValueBy(1m);
        }
    }
}