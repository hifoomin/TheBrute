using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBrute.Cards.Uncommons
{
    internal class Zone : TheBruteCard
    {
        public Zone() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
        {
        }

        public override bool GainsBlock => true;

        private bool PlayedPowerThisTurn()
        {
            return CombatManager.Instance.History.CardPlaysFinished
            .Any(e =>
            e.HappenedThisTurn(CombatState) &&
            e.CardPlay.Card.Owner == Owner &&
            e.CardPlay.Card.Type == CardType.Power);
        }

        protected override bool ShouldGlowGoldInternal => PlayedPowerThisTurn();

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CalculationBaseVar(7m),
            new CalculationExtraVar(4m),
            new CalculatedBlockVar(ValueProp.Move).WithMultiplier((CardModel card, Creature? _) =>
            {
                var playedPowerThisTurn = CombatManager.Instance.History.CardPlaysFinished
                                          .Any(e =>
                                          e.HappenedThisTurn(card.CombatState) &&
                                          e.CardPlay.Card.Owner == card. Owner &&
                                          e.CardPlay.Card.Type == CardType.Power);

                return playedPowerThisTurn ? 1m : 0m;
            })
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.CalculatedBlock.Calculate(cardPlay.Target), ValueProp.Move, cardPlay);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.CalculationBase.UpgradeValueBy(3m);
        }
    }
}