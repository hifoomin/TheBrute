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

            var max = Math.Min(Amount, CardPile.MaxCardsInHand - PileType.Hand.GetPile(player).Cards.Count);
            if (max > 0)
            {
                await CardPileCmd.Add(await CardSelectCmd.FromCombatPile(choiceContext, PileType.Draw.GetPile(player), player, new CardSelectorPrefs(SelectionScreenPrompt, 0, max)), PileType.Hand);
                await PowerCmd.Remove(this);
            }
        }
    }
}