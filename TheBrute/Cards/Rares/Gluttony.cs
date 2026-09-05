#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace TheBrute.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Gluttony : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Gluttony() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        protected override bool HasEnergyCostX => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(11)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var repeats = ResolveEnergyXValue();
            if (IsUpgraded)
            {
                repeats++;
            }

            for (var i = 0; i < repeats; i++)
            {
                await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
            }
        }
    }
}