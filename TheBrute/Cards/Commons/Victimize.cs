#region

using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

#endregion

namespace TheBrute.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Victimize : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Victimize() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<VulnerablePower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<VulnerablePower>(1m),
            new GoldVar(2),
            new ExhaustiveVar(3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);

            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);

            await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Vulnerable.UpgradeValueBy(1m);
        }
    }
}