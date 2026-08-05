using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
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
using TheBrute.Powers;

namespace TheBrute.Cards.Rares
{
    internal class Autothysis : TheBruteCard
    {
        public Autothysis() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        protected override bool IsPlayable => MaxHpTracker.GetTotalMaxHpLostThisCombat(Owner.Creature) >= DynamicVars["MaxHpLostRequirement"].BaseValue;
        protected override bool ShouldGlowRedInternal => !IsPlayable;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DynamicVar("MaxHpLostRequirement", 4m),
            new BlockVar(15m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // just in case idk lmfaoo this game badly coded
            if (!IsPlayable)
            {
                return;
            }

            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5m);
            // DynamicVars["MaxHpLostRequirement"].UpgradeValueBy(-1m);
        }
    }
}