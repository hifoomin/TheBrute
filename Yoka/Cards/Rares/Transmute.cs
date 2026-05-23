using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoka.Cards.Commons;
using Yoka.Powers;

namespace Yoka.Cards.Rares
{
    internal class Transmute : YokaCard
    {
        public Transmute() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
        {
        }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];

        /*
        public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            CardKeyword.Exhaust
        ];
        */

        protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new MaxHpVar(2)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), null, this))
            {
                if (!card.EnergyCost.CostsX && card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                {
                    var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                    var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);

                    if (!card.hasTransmutedKeyword())
                    {
                        Main.Logger.Warn("added transmuted keyword");
                        card.AddKeyword(Yoka.Cards.Keywords.transmutedKeyword);
                    }

                    if (cardVars.TryGetValue("Transmuted", out var existingVar) && existingVar is TransmutedVar existingTransmuted)
                    {
                        Main.Logger.Warn("added ONTO EXISTINGTGFDKHG EGDSAGSD transmuted var to card");
                        Main.Logger.Warn("Existing transmuted var before: " + existingTransmuted.BaseValue);
                        cardVars["Transmuted"] = new TransmutedVar(existingTransmuted.BaseValue + DynamicVars.MaxHp.BaseValue);
                        Main.Logger.Warn("Existing transmuted var AFTERERERERERER: " + cardVars["Transmuted"].BaseValue);
                    }
                    else
                    {
                        Main.Logger.Warn("added NEWWW transmuted var to card");
                        cardVars["Transmuted"] = new TransmutedVar(DynamicVars.MaxHp.BaseValue);
                    }
                }

                card.BaseReplayCount++;
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}