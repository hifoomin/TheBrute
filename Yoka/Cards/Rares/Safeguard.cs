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
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Safeguard : YokaCard
    {
        public Safeguard() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Retain
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(7m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new DynamicVar("RetainBlock", 4m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            // DynamicVars.Block.BaseValue = 7m;
        }

        public override Task BeforeFlush(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner)
            {
                return Task.CompletedTask;
            }
            CardPile? pile = Pile;
            if (pile == null || pile.Type != PileType.Hand)
            {
                return Task.CompletedTask;
            }

            DynamicVars.Block.UpgradeValueBy(DynamicVars["RetainBlock"].BaseValue);

            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            DynamicVars["RetainBlock"].UpgradeValueBy(1m);
        }
    }
}