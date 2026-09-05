#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

#endregion

namespace TheBrute.Cards.Commons
{
#pragma warning disable STS001 // Symbol missing localization

    internal class Adapt : TheBruteCard
#pragma warning restore STS001 // Symbol missing localization
    {
        public Adapt() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
        {
        }

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(2),
            new("CardUpgrade", 1m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);

            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

            static bool filter(CardModel c)
            {
                return c.IsUpgradable;
            }

            var cards = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, DynamicVars["CardUpgrade"].IntValue), context: choiceContext, player: Owner, filter: filter, source: this);
            foreach (var card in cards)
            {
                CardCmd.Upgrade(card);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["CardUpgrade"].UpgradeValueBy(1m);
        }
    }
}