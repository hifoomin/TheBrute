using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards;

namespace Yoka.Cards.Uncommons
{
    internal class Leverage : YokaCard
    {
        public Leverage() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override bool ShouldGlowGoldInternal => Utils.TookUnblockedDamageLastTurn(Owner);

        protected override bool ShouldGlowRedInternal => !Utils.TookUnblockedDamageLastTurn(Owner);

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new EnergyVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Utils.TookUnblockedDamageLastTurn(Owner))
            {
                await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
                await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}