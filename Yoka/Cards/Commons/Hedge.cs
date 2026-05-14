using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards;

namespace Yoka.Cards.Commons
{
    internal class Hedge : YokaCard
    {
        public Hedge() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        private int lastGold;

        private bool changedGoldThisTurn => Owner.Gold != lastGold;

        protected override bool ShouldGlowGoldInternal => Owner.Gold != lastGold;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(4m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new RepeatVar(1)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            int blockGains = 1;
            if (changedGoldThisTurn)
            {
                blockGains += DynamicVars.Repeat.IntValue;
            }

            for (int i = 0; i < blockGains; i++)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            }
        }

        public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
        {
            lastGold = Owner.Gold;
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1m);
        }
    }
}