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

namespace Yoka.Cards.Commons
{
    internal class Weaponize : YokaCard
    {
        public Weaponize() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<VigorPower>()];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new PowerVar<StrengthPower>(0m),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedVigor").WithMultiplier((card, _) =>
            {
                var creature = card.Owner.Creature;
                var totalThorns = creature.GetPowerAmount<ThornsPower>() + creature.GetPowerAmount<TemporaryThornsPower>();

                return totalThorns;
            }),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<StrengthPower>(choiceContext, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
            await PowerCmd.Apply<VigorPower>(choiceContext, Owner.Creature, ((CalculatedVar)DynamicVars["CalculatedVigor"]).Calculate(Owner.Creature), Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Strength.UpgradeValueBy(1m);
        }
    }
}