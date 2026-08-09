#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

#endregion

namespace TheBrute.Powers
{
    internal class AttunePower : TheBrutePower
    {
        public override PowerType Type => PowerType.Buff;

        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStartLate(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner.Player)
            {
                return;
            }

            var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
            var cardsIn = PileType.Draw.GetPile(player).Cards.ToList();
            var cardModel = (await CardSelectCmd.FromSimpleGrid(choiceContext, cardsIn, player, prefs)).FirstOrDefault();
            if (cardModel != null)
            {
                await CardPileCmd.Add(cardModel, PileType.Hand);
                await PowerCmd.Remove(this);
            }
        }
    }
}