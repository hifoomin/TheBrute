#region

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Supremacy : TheBruteCard
    {
        public Supremacy() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(5m, ValueProp.Move),
            new CardsVar(3)
        ];

        public override bool GainsBlock => true;

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        public override async Task AfterCardPlayedLate(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner == Owner && cardPlay.Card.Type == CardType.Attack && Pile.Type != PileType.Hand)
            {
                var attacksPlayedThisTurn = CombatManager.Instance.History.CardPlaysFinished.Count(e => e.HappenedThisTurn(CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == Owner);
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