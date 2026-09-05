#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using TheBrute.Powers;

#endregion

namespace TheBrute.Cards.AprilFools
{
    internal class Bitesnake : AprilFoolsCard
    {
        public Bitesnake() : base(2, CardType.Skill, CardRarity.Ancient, TargetType.AnyEnemy, SpecialEventManager.IsAprilFools(DateTime.Today), SpecialEventManager.IsAprilFools(DateTime.Today))
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PoisonPower>(),
            HoverTipFactory.FromPower<RetainPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<RetainPower>(7m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await PowerCmd.Apply<RetainPower>(choiceContext, cardPlay.Target, DynamicVars["RetainPower"].BaseValue, Owner.Creature, this);
        }
    }
}