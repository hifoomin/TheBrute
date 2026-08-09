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
    internal class BloodCoagulant : TheBruteCard
    {
        public BloodCoagulant() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(1m),
            new PowerVar<ThornsPower>(6m),
            new PowerVar<TemporaryThornsUpPower>(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, DynamicVars.MaxHp.BaseValue, true);

            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<TemporaryThornsUpPower>(choiceContext, Owner.Creature, DynamicVars["TemporaryThornsUpPower"].BaseValue, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ThornsPower"].UpgradeValueBy(2m);
            DynamicVars["TemporaryThornsUpPower"].UpgradeValueBy(1m);
        }
    }
}