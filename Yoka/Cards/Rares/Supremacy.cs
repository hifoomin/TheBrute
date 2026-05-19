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
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Supremacy : YokaCard
    {
        public Supremacy() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(5m, MegaCrit.Sts2.Core.ValueProps.ValueProp.Move),
            new CardsVar(3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Attack && Pile.Type != PileType.Hand)
            {
                var attacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == Owner);
                if (attacksPlayedThisTurn % DynamicVars.Cards.IntValue == 0)
                {
                    await CardPileCmd.Add(this, PileType.Hand);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2m);
        }
    }
}