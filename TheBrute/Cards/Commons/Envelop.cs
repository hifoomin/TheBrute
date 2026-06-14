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
using TheBrute.Cards;
using static BaseLib.Utils.BetaMainCompatibility;
using static Godot.OpenXRCompositionLayer;

namespace TheBrute.Cards.Commons
{
    internal class Envelop : TheBruteCard
    {
        public Envelop() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<PlatingPower>()
        ];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new PowerVar<PlatingPower>(3m),
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<PlatingPower>(choiceContext, Owner.Creature, DynamicVars["PlatingPower"].BaseValue, Owner.Creature, this);

            var cards = (await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue), null, this));
            foreach (var card in cards)
            {
                await CardPileCmd.Add(card, PileType.Draw, CardPilePosition.Top);
            }
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}