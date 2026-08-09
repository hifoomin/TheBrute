#region

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Safeguard : TheBruteCard
    {
        public Safeguard() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Retain
        ];

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new BlockVar(8m, ValueProp.Move),
            new("RetainBlock", 4m)
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
            var pile = Pile;
            if (pile == null || pile.Type != PileType.Hand)
            {
                return Task.CompletedTask;
            }

            DynamicVars.Block.UpgradeValueBy(DynamicVars["RetainBlock"].BaseValue);

            return Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            // DynamicVars.Block.UpgradeValueBy(3m);
            DynamicVars["RetainBlock"].UpgradeValueBy(2m);
        }
    }
}