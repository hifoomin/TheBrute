using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Yoka.Cards.Rares
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Gluttony : YokaCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Gluttony() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override bool HasEnergyCostX => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
           CardKeyword.Exhaust
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new GoldVar(15)
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            Yoka.Cards.Tags.goldRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var repeats = ResolveEnergyXValue();
            if (IsUpgraded)
            {
                repeats++;
            }

            for (int i = 0; i < repeats; i++)
            {
                await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, Owner);
            }
        }
    }
}