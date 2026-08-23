#region

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

#endregion

namespace TheBrute.Cards.Rares
{
    internal class Transmute : TheBruteCard
    {
        public Transmute() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new CardsVar(1),
            new MaxHpVar(3m)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var cards = await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, DynamicVars.Cards.IntValue), context: choiceContext, player: Owner, filter: null, source: this);

            foreach (var card in cards)
            {
                /*
                var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);
                if (cardVars == null)
                {
                    return;
                }

                if (!card.hasTransmutedKeyword())
                {
                    // Main.Logger.Warn("added transmuted keyword");
                    card.AddKeyword(TheBrute.Cards.Keywords.transmutedKeyword);
                }

                if (cardVars.TryGetValue("Transmuted", out var existingVar) && existingVar is TransmutedVar existingTransmuted)
                {
                    // Main.Logger.Warn("added ONTO EXISTINGTGFDKHG EGDSAGSD transmuted var to card");
                    // Main.Logger.Warn("Existing transmuted var before: " + existingTransmuted.BaseValue);
                    cardVars["Transmuted"] = new TransmutedVar(existingTransmuted.BaseValue + DynamicVars.MaxHp.BaseValue);
                    // Main.Logger.Warn("Existing transmuted var AFTERERERERERER: " + cardVars["Transmuted"].BaseValue);
                }
                else
                {
                    // Main.Logger.Warn("added NEWWW transmuted var to card");
                    cardVars["Transmuted"] = new TransmutedVar(DynamicVars.MaxHp.BaseValue);
                }
                */

                // card.AddModifier(Cards.MaxHpLossModifier.);

                MaxHpLossModifier.AddTo(card, DynamicVars.MaxHp.BaseValue);

                card.BaseReplayCount++;
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.MaxHp.UpgradeValueBy(-1m);
        }
    }
}