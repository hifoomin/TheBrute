using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Unflinching : YokaCard
    {
        public Unflinching() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<ThornsPower>(),
            HoverTipFactory.FromPower<PlatingPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(0m),
            new CalculationExtraVar(0.75m),
            new CalculatedVar("CalculatedPlating").WithMultiplier((card, _) =>
            {
                var creature = card.Owner.Creature;
                var totalThorns = creature.GetPowerAmount<ThornsPower>();

                return totalThorns;
            })
        ];

        protected override HashSet<CardTag> CanonicalTags => new
        ([
            Yoka.Cards.Tags.thornsRelated
        ]);

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["CalculatedPlating"]).Calculate(Owner.Creature), Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationExtra.UpgradeValueBy(0.25m);
        }
    }
}