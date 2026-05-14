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
            new HpLossVar(3)
        ];

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var filter = (CardModel card) => (!card.DynamicVars.TryGetValue("HpLoss", out var hpLoss) || hpLoss.BaseValue <= 0) || (!card.DynamicVars.TryGetValue("Heal", out var heal) || heal.BaseValue > 0);
            foreach (CardModel card in await CardSelectCmd.FromHand(choiceContext, Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1), filter, this))
            {
                if (!card.EnergyCost.CostsX && card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
                {
                    // CardCmd.ApplyKeyword(item, CardKeyword.Exhaust);
                    // item.DynamicVars.HpLoss.BaseValue += DynamicVars.HpLoss.BaseValue;
                    var vars = AccessTools.Field(typeof(DynamicVarSet), "_vars");

                    var cardVars = (Dictionary<string, DynamicVar>)vars.GetValue(card.DynamicVars);

                    cardVars["HpLoss"] = new HpLossVar(DynamicVars.HpLoss.BaseValue);
                    // Main.Logger.Warn("transmute onplay: trying to add keyword and hp loss");
                    card.AddKeyword(Yoka.Keywords.hpLossKeyword);
                    // Main.Logger.Warn("transmute onplay: cards new hploss value is " + vars["HpLoss"].BaseValue);
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